using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NPS.Loot
{
    public class UILoot : UIView, IPopup
    {
        [SerializeField] private DictionaryMachine machines;
        [SerializeField] private GameEventLootType evtLootReward;
        [SerializeField] private Transform lootContent;
        [SerializeField] private List<ItemData> lstLootItem;

        private readonly Dictionary<LootType, ItemData> dicLootItem = new Dictionary<LootType, ItemData>();
        private readonly List<UIItem> listItems = new List<UIItem>();
        private Dictionary<Rarity, int> countRarity = new Dictionary<Rarity, int>();
        private List<LootData> rewards = new List<LootData>();
        private int _diamond = 0;

        private ChestsSave _chestsSave;
        private CollectionSave _collectionSave;
        private SpecialSave _specialSave;
        private UserSave _userSave;
        private LevelUniverseEntity _levelUniverse;
        private CollectionDishSave _collectionDishSave;
        private ClothesSave _clothesSave;
        private RarityTable _rarityTable;
        private Action _callback;
        private Action _vfxAction;

        private readonly Queue<object> queue = new Queue<object>();
        private bool _allCurrencyMachine = false;
        private bool _isShowVfxAfterClose = false;

        private void Awake()
        {
            _chestsSave = DataManager.Save.Chest;
            _collectionSave = DataManager.Save.Collection;
            _specialSave = DataManager.Save.Special;
            _userSave = DataManager.Save.User;
            _levelUniverse = DataManager.Base.ZoneUniverse.LevelMode;
            _rarityTable = DataManager.Base.Rarity;
            _collectionDishSave = DataManager.Save.CollectionDish;
            _clothesSave = DataManager.Save.Clothes;
        }

        protected override void Init()
        {
            base.Init();
            dicLootItem.Clear();
            foreach (var item in lstLootItem.Where(item => !dicLootItem.ContainsKey(item.Type)))
            {
                dicLootItem.Add(item.Type, item);
            }
        }

        public override void Show(object obj = null)
        {
            base.Show(obj);
            if (obj is UILootData viewData)
            {
                queue.Enqueue(obj);
                if (queue.Count == 1)
                {
                    var reward = viewData.Rewards;
                    this._callback = viewData.HideCallback;
                    this._allCurrencyMachine = viewData.AllCurrencyMachine;

                    var scale = Constant.ScaleReward.ContainsKey(viewData.Rewards.Count)
                        ? Constant.ScaleReward[viewData.Rewards.Count]
                        : 1f;
                    lootContent.localScale = new Vector3(scale, scale, 1);
                    Reward(reward, viewData.Location, viewData.LocationId, _allCurrencyMachine, viewData.ShowInfor);
                    HideReward();
                    PoolRewards(this.rewards);
                }
            }
        }

        #region REWARD

        public void Reward(List<LootData> reward, string location, string locationId, bool allCurrency = false,
            bool isLoot = true, bool skip = false)
        {
            _diamond = 0;
            var loots = reward.Merge();
            this.rewards.Clear();

            foreach (var rarity in (Rarity[])Enum.GetValues(typeof(Rarity)))
            {
                countRarity[rarity] = 0;
            }

            foreach (var loot in loots)
            {
                switch (loot.Type)
                {
                    case LootType.Currency:
                        ProcessCurrencyReward(loot);
                        break;
                    case LootType.Equipment:
                        ProcessEquipmentReward(loot);
                        break;
                    case LootType.Key:
                        ProcessKeyReward(loot);
                        break;
                    case LootType.Special:
                        ProcessSpecialReward(loot);
                        break;
                    case LootType.Dish:
                        ProcessDishReward(loot);
                        break;
                    case LootType.Potion:
                        ProcessPotionReward(loot);
                        break;
                    case LootType.Chest:
                        break;
                    case LootType.Clothes:
                        ProcessClothesReward(loot);
                        break;
                    case LootType.PuzzleFragment:
                        ProcessPuzzleFragmentReward(loot);
                        break;
                    case LootType.PuzzleEnvelope:
                        ProcessPuzzleFragmentEnvelopeReward(loot);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            this.rewards = this.rewards.Merge();
            _collectionSave.SetRewards(new UILootData(this.rewards, location, locationId, null, false, false));
            _collectionSave.Save();

            if (isLoot)
            {
                Loot(this.rewards, location, locationId, true, true, allCurrency, skip);
            }
        }

        private void ProcessCurrencyReward(LootData loot)
        {
            var currency = loot.Data.Clone() as CurrencyData;
            if (currency != null)
            {
                this.rewards.Add(new LootData()
                {
                    Type = LootType.Currency,
                    Data = new CurrencyData()
                    {
                        Type = currency.Type,
                        Value = currency.Value,
                    }
                });
            }
        }

        private void ProcessEquipmentReward(LootData loot)
        {
            var equip = loot.Data.Clone() as EquipmentData;
            if (equip != null)
            {
                var max = _rarityTable.Dictionary[equip.Rarity].MaxEquipment;
                var id = equip.Id;

                if (max == -1 || countRarity[equip.Rarity] < max)
                {
                    for (var j = 0; j < equip.Value; j++)
                    {
                        if (equip.Id == -1)
                            id = _levelUniverse.GetRandom(equip.Rarity);

                        this.rewards.Add(new LootData()
                        {
                            Type = LootType.Equipment,
                            Data = new EquipmentData()
                            {
                                Id = id,
                                Value = 1
                            }
                        });

                        countRarity[equip.Rarity]++;
                    }
                }
            }
        }

        private void ProcessKeyReward(LootData loot)
        {
            var key = loot.Data.Clone() as KeyData;
            if (key != null)
            {
                this.rewards.Add(new LootData()
                {
                    Type = LootType.Key,
                    Data = new KeyData()
                    {
                        Type = key.Type,
                        Value = key.Value,
                    }
                });
            }
        }

        private void ProcessSpecialReward(LootData loot)
        {
            var special = loot.Data.Clone() as SpecialRwData;
            if (special != null)
            {
                var id = special.Id;
                for (var j = 0; j < special.Value; j++)
                {
                    if (special.Id == -1)
                    {
                        id = DataManager.Base.Special.GetRandom(special.Rarity);
                    }

                    this.rewards.Add(new LootData()
                    {
                        Type = LootType.Special,
                        Data = new SpecialRwData()
                        {
                            Id = id,
                            Value = 1
                        }
                    });
                }
            }
        }

        private void ProcessDishReward(LootData loot)
        {
            var dish = loot.Data.Clone() as DishCrData;
            if (dish != null)
            {
                var idDish = dish.Id;

                for (var j = 0; j < dish.Value; j++)
                {
                    if (dish.Id == -1)
                    {
                        idDish = DataManager.Base.Dish.GetRandom(dish.Rarity);
                    }

                    this.rewards.Add(new LootData()
                    {
                        Type = LootType.Dish,
                        Data = new DishCrData()
                        {
                            Id = idDish,
                            Value = 1
                        }
                    });
                }
            }
        }

        private void ProcessClothesReward(LootData loot)
        {
            var clothes = loot.Data.Clone() as ClothesData;
            if (clothes != null)
            {
                var clothesId = clothes.Id;

                for (var j = 0; j < clothes.Value; j++)
                {
                    this.rewards.Add(new LootData()
                    {
                        Type = LootType.Clothes,
                        Data = new ClothesData()
                        {
                            Id = clothesId,
                            Value = 1
                        }
                    });
                }
            }
        }

        private void ProcessPotionReward(LootData loot)
        {
            var potion = loot.Data.Clone() as PotionData;
            if (potion != null)
            {
                this.rewards.Add(new LootData()
                {
                    Type = LootType.Potion,
                    Data = new PotionData()
                    {
                        Type = potion.Type,
                        Value = potion.Value,
                    }
                });
            }
        }

        private void ProcessPuzzleFragmentReward(LootData loot)
        {
            var fragment = loot.Data.Clone() as PuzzleFragmentData;

            if (fragment != null)
            {
                this.rewards.Add(new LootData()
                {
                    Type = LootType.PuzzleFragment,
                    Data = new PuzzleFragmentData()
                    {
                        Collect = fragment.Collect,
                        Image = fragment.Image,
                        Id = fragment.Id
                    }
                });
            }
        }

        private void ProcessPuzzleFragmentEnvelopeReward(LootData loot)
        {
            var envelope = loot.Data.Clone() as PuzzleEnvelopeData;

            if (envelope != null)
            {
                this.rewards.Add(new LootData()
                {
                    Type = LootType.PuzzleFragment,
                    Data = new PuzzleEnvelopeData()
                    {
                        Type = envelope.Type,
                        Value = envelope.Value,
                    }
                });
            }
        }

        private void PoolRewards(List<LootData> rewards)
        {
            foreach (var item in rewards)
            {
                if (dicLootItem.TryGetValue(item.Type, out var lootItem))
                {
                    var obj = PoolManager.S.Spawn(lootItem.Item, lootContent);
                    if (item.Type == LootType.Currency)
                    {
                        obj.Set(new Tuple<ILootData, double>(item.Data,
                            _allCurrencyMachine ? machines.AllCurrency() : -1));
                    }
                    else
                    {
                        obj.Set(item.Data);
                    }

                    listItems.Add(obj);
                }
            }
        }

        private void HideReward()
        {
            foreach (var item in listItems)
            {
                PoolManager.S.Despawn(item.gameObject);
            }

            listItems.Clear();
        }

        #endregion

        #region LOOT

        public void Loot(List<LootData> rewards, string location, string locationId, bool isEquip = true,
            bool isSpecial = true, bool allCurrencyMachine = false, bool skip = false)
        {
            this._allCurrencyMachine = allCurrencyMachine;
            foreach (var t in rewards)
            {
                Loot(t, location, locationId, isEquip, isSpecial, allCurrencyMachine, skip);
            }
        }

        public void Loot(LootData reward, string location, string locationId, bool isEquip = true,
            bool isSpecial = true, bool allCurrencyMachine = false, bool skip = false)
        {
            if (reward == null)
                return;

            switch (reward.Type)
            {
                case LootType.Currency:
                    ProcessCurrencyLoot(reward.Data as CurrencyData, location, locationId, allCurrencyMachine);
                    break;
                case LootType.Chest:
                    ProcessChestLoot(reward.Data as ChestData);
                    break;
                case LootType.Equipment:
                    ProcessEquipmentLoot(reward.Data as EquipmentData, isEquip, skip);
                    break;
                case LootType.Special:
                    ProcessSpecialLoot(reward.Data as SpecialRwData, isSpecial, skip, location, locationId);
                    break;
                case LootType.Key:
                    ProcessKeyLoot(reward.Data as KeyData);
                    break;
                case LootType.Dish:
                    ProcessDishLoot(reward.Data as DishCrData, skip);
                    break;
                case LootType.Potion:
                    ProcessPotionLoot(reward.Data as PotionData);
                    break;
                case LootType.Clothes:
                    ProcessClothesLoot(reward.Data as ClothesData);
                    break;
                case LootType.PuzzleFragment:
                    ProcessFragmentLoot(reward.Data as PuzzleFragmentData, location, locationId);
                    break;
                case LootType.PuzzleEnvelope:
                    ProcessFragmentEnvelopeLoot(reward.Data as PuzzleEnvelopeData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            evtLootReward?.Raise(reward.Type);
        }

        private void ProcessCurrencyLoot(CurrencyData currency, string location, string locationId,
            bool allCurrencyMachine)
        {
            if (currency.Type is CurrencyType.TicketMachine or CurrencyType.LeafClover
                or CurrencyType.TicketMachine_UniverseSpecial or CurrencyType.LeafClover_UniverseSpecial)
            {
                _userSave.AddCurrency(currency.Type, currency.Value, location, locationId);
                _userSave.Save();
            }
            else
            {
                _vfxAction = () => MainGameScene.S.Fix.Loot(currency, location, locationId, null,
                    posCurrency(currency.Type), null, null, allCurrencyMachine);
                if (!_isShowVfxAfterClose)
                {
                    _vfxAction?.Invoke();
                }
            }
        }

        private void ProcessChestLoot(ChestData chest)
        {
            _chestsSave.ReciveReward(chest);
            MainGameScene.S.Show<UIUnlockChest>(new Tuple<OpenChestType, ChestData>(OpenChestType.Reward, chest));
        }

        private void ProcessEquipmentLoot(EquipmentData equipment, bool isEquip, bool skip)
        {
            var isShow = MainGameScene.S.IsShow<UIInforEquipment>();
            var equip = equipment.Clone() as EquipmentData;

            var count = equip.Value;
            if (equip.Id == -1)
            {
                equip.Id = _levelUniverse.GetRandom(equip.Rarity);
                count = 1;

                var val = !isShow ? 1 : 0;
                if (equip.Value > val)
                {
                    _collectionSave.ReciveReward(-1, new RewardEquipmentSave()
                    {
                        Type = equip.Rarity,
                        Value = (int)equip.Value - val,
                    });
                }
            }

            var rs = _collectionSave.ReciveReward(new EquipmentData()
            {
                Id = equip.Id,
                Value = count
            });

            if (!isShow)
            {
                if (isEquip)
                    MainGameScene.S.Show<UIInforEquipment>(
                        new Tuple<int, Action, bool, bool, int, bool>(equip.Id, null, true, rs, -1, skip));
                else
                {
                    var rarity = _rarityTable.Dictionary[_levelUniverse.Equipment[equip.Id].Rarity];
                    if (_collectionSave.Equipment[equip.Id].Level == rarity.PriceEquipment.Count - 1)
                    {
                        _diamond += rarity.DiamondEquipment;
                    }
                }
            }
            else
                _collectionSave.ReciveReward(equip.Id, new RewardEquipmentSave()
                {
                    Type = equip.Rarity,
                    Value = (int)equip.Value
                });

            DataManager.Save.AchievementLotto.Tracking(LottoMissionType.OpenEquipment);
        }

        private void ProcessSpecialLoot(SpecialRwData special, bool isSpecial, bool skip, string location,
            string locationId)
        {
            var isShowSpecial = MainGameScene.S.IsShow<UIOpenSpecialOld>() ||
                                MainGameScene.S.IsShow<UIOpenSpecialNew>();
            var specialValue = special.Value;

            if (special.Id == -1)
            {
                special.Id = DataManager.Base.Special.GetRandom(special.Rarity);
                specialValue = 1;

                var value = !isShowSpecial ? 1 : 0;
                if (special.Value > value)
                    _specialSave.ReciveReward(-1, new RewardSpecialSave()
                    {
                        Type = special.Rarity,
                        Value = (int)special.Value - value,
                    });
            }

            var save = _specialSave.Dictionary[special.Id];
            _specialSave.ReciveReward(new SpecialRwData()
            {
                Id = special.Id,
                Value = specialValue
            }, location, locationId);
            if (!isShowSpecial)
            {
                var open = new OpenSpecial()
                {
                    Id = special.Id,
                    Star = save.Star,
                    Count = save.Count,
                    MilestoneStar = save.MilestoneStar,
                    Max = save.MilestoneStar == 4 && save.Star >= DataManager.Base.Special.GetStarTier(special.Id, 4),
                };

                if (isSpecial)
                {
                    if (open.Star == 0 && open.MilestoneStar == 0)
                    {
                        MainGameScene.S.Show<UIOpenSpecialNew>(new Tuple<OpenSpecial, bool>(open, skip));
                    }
                    else
                    {
                        MainGameScene.S.Show<UIOpenSpecialOld>(new Tuple<OpenSpecial, bool>(open, skip));
                    }
                }
                else
                {
                    if (save.MilestoneStar == 4 && save.Star >= DataManager.Base.Special.GetStarTier(special.Id, 4))
                    {
                        _diamond += _rarityTable.Dictionary[DataManager.Base.Special.Dictionary[special.Id].Rarity]
                            .DiamondSpecial;
                    }
                }
            }
            else
            {
                _specialSave.ReciveReward(special.Id, new RewardSpecialSave()
                {
                    Type = special.Rarity,
                    Value = (int)special.Value,
                });
            }
        }

        private void ProcessKeyLoot(KeyData key)
        {
            _vfxAction = () =>
                MainGameScene.S.Fix.Loot(key.Value, key.Type.ToString(), null, MainGameScene.S.Main.PosShop, null);
            if (!_isShowVfxAfterClose)
            {
                _vfxAction?.Invoke();
            }

            _userSave.IncreaseKey(key);
            _userSave.Save();
        }

        private void ProcessDishLoot(DishCrData dish, bool skip)
        {
            var isShowDish = MainGameScene.S.IsShow<UIOpenDish>();
            var countDish = dish.Value;

            if (dish.Id == -1)
            {
                dish.Id = _levelUniverse.GetRandom(dish.Rarity);
                countDish = 1;

                var val = !isShowDish ? 1 : 0;
                if (dish.Value > val)
                {
                    _collectionDishSave.ReciveReward(-1, new RewardDishSave()
                    {
                        Type = dish.Rarity,
                        Value = (int)dish.Value - val,
                    });
                }
            }

            if (!isShowDish)
            {
                _collectionDishSave.ReciveReward(new DishsSave()
                {
                    Id = dish.Id,
                    Level = 0,
                });
                MainGameScene.S.Show<UIOpenDish>(dish.Id);
            }
            else
            {
                _collectionDishSave.ReciveReward(dish.Id, new RewardDishSave()
                {
                    Type = dish.Rarity,
                    Value = (int)dish.Value
                });
            }
        }

        private void ProcessPotionLoot(PotionData potion)
        {
            _vfxAction = () =>
                MainGameScene.S.Fix.Loot(potion.Value, potion.Type.ToString(), null, MainGameScene.S.Main.PosShop,
                    null);
            if (!_isShowVfxAfterClose)
            {
                _vfxAction?.Invoke();
            }

            DataManager.Save.Creative.IncreaseCurrency(potion.Type, potion.Value);
            DataManager.Save.Creative.Save();
        }

        private void ProcessClothesLoot(ClothesData clothes)
        {
            var isShowDish = MainGameScene.S.IsShow<UIRewardClothes>();

            if (!isShowDish)
            {
                _clothesSave.ReciveReward(new ClothesDataSave()
                {
                    Id = clothes.Id,
                    Level = 0,
                });
                MainGameScene.S.Show<UIRewardClothes>(clothes.Id);
            }
            else
            {
                _clothesSave.ReciveReward(clothes.Id, new RewardClothesSave()
                {
                    Type = clothes.Rarity,
                    Value = (int)clothes.Value
                });
            }
        }

        private void ProcessFragmentLoot(PuzzleFragmentData fragment, string location, string locationId)
        {
            DataManager.Save.Puzzle.Collect[fragment.Collect].InCreaseFragment(location, locationId, fragment);
        }

        private void ProcessFragmentEnvelopeLoot(PuzzleEnvelopeData fragment)
        {
        }

        #endregion

        private Transform posCurrency(CurrencyType type)
        {
            Transform pos;
            switch (type)
            {
                case CurrencyType.Point2:
                    pos = MainGameScene.S.Main.IconEventSpecial.TranIcon;
                    break;
                case CurrencyType.Point:
                case CurrencyType.Ticket:
                    pos = MainGameScene.S.Main.IconEvent.TranIcon;
                    break;
                case CurrencyType.Water:
                {
                    pos = GameManager.S.Mode != GameMode.MiniGame ? MainGameScene.S.Main.PosShop : null;
                }
                    break;
                case CurrencyType.BoosterX5:
                case CurrencyType.BoosterX2:
                case CurrencyType.JackPot:
                {
                    pos = MainGameScene.S.Main.PosShop;
                }
                    break;
                case CurrencyType.Shovel_Easter:
                case CurrencyType.Shovel_HelloSummer:
                {
                    pos = null;
                }
                    break;
                default:
                    pos = null;
                    break;
            }

            return pos;
        }

        public override void Hide()
        {
            base.Hide();

            // Action
            this._callback?.Invoke();
            this._callback = null;

            // Vfx
            if (_isShowVfxAfterClose)
            {
                this._vfxAction?.Invoke();
                this._vfxAction = null;
                this._isShowVfxAfterClose = false;
            }

            HideReward();

            if (_diamond > 0)
            {
                Loot(new LootData()
                {
                    Type = LootType.Currency,
                    Data = new CurrencyData()
                    {
                        Type = CurrencyType.Diamond,
                        Value = _diamond
                    }
                }, Location.CardMax_PopupRewards, LocationId.Empty);

                _diamond = 0;
                AudioManager.S.PlayOneShoot("Expense_Coin");
            }

            if (queue is { Count: > 0 })
            {
                queue.Dequeue();

                if (queue.Count > 0)
                {
                    Show(queue.Dequeue());
                }
            }
        }

        public void ShowWithVfxAfterClose(object obj = null)
        {
            this._isShowVfxAfterClose = true;
            Show(obj);
        }
    }
}