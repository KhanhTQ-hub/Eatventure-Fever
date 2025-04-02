using NPS.Tutorial;

public class TutorialManager : Manager
{
    private MapSave _mapSave;

    public override void InitData()
    {
        _mapSave = DataManager.Save?.Map;

        base.InitData();
    }

    protected override bool iConditionInit(int tut)
    {
        var remote = DataManager.Save.RemoteConfig;
        var mode = GameManager.S.Mode;

        switch (tut)
        {
            // Lock mh khi vào game lần đầu
            // Tutirual unlock, update machine
            case 1:
                return _mapSave.Zone == 0 && _mapSave.Level == 0;
            case 2:
                if (!save.Complete.Contains(tut - 1)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 0;
            // Tut upgrades chung, boost and renovate
            case 3:
                if (!save.Complete.Contains(tut - 1)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 0;
            case 31:
                if (!save.Complete.Contains(3)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 0;
            // TutTaskMain
            case 32:
                if (!save.Complete.Contains(3)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 0;
            case 4:
                if (!save.Complete.Contains(tut - 1)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 0 && remote.UiMainGameConfigValue == "a";
            case 41:
                if (!save.Complete.Contains(3)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 0 && remote.UiMainGameConfigValue == "b";
            case 42:
                if (!save.Complete.Contains(41)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 0;
            case 5:
                if (!save.Complete.Contains(3)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 0;
            // Tut Merge human
            case 61:
                if (!save.Complete.Contains(5)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 1;
            case 611:
                if (!save.Complete.Contains(61)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 1;
            case 62:
                if (!save.Complete.Contains(611)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 1;
            case 7:
                if (!save.Complete.Contains(62)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 1;
            // Tut Tip
            case 8:
                if (!save.Complete.Contains(tut - 1)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 1;
            // Tut Collection
            case 9:
                if (!save.Complete.Contains(tut - 1)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 2;
            // Tut show popup starter pack, setting, event
            case 91:
                return _mapSave.Zone == 0 && _mapSave.Level == 2;
            case 92:
                return _mapSave.Zone == 0 && _mapSave.Level == 2;
            case 93:
                return _mapSave.Zone == 0 && _mapSave.Level == 2;
            case 94:
                return _mapSave.Zone == 0 && _mapSave.Level == 2;
            case 95:
                return _mapSave.Zone == 0 && _mapSave.Level == 3;
            // Tut Shop
            case 10:
                if (!save.Complete.Contains(tut - 1)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 2;
            case 101:
                if (!save.Complete.Contains(10)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 2;
            case 102:
                if (!save.Complete.Contains(101)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 2;
            // Tut Elon musk
            case 11:
                if (save.CurTut == 101) return false;
                switch (_mapSave.Zone)
                {
                    case 0 when _mapSave.Level == 1 && save.Complete.Contains(8):
                    case 0 when _mapSave.Level > 1 && save.Complete.Contains(10):
                        return true;
                    default:
                        return false;
                }
            // Tut Miner   
            case 12:
                if (mode != GameMode.Normal) return false;
                if (save.CurTut == 101) return false;
                if (!save.Complete.Contains(11) || !save.Complete.Contains(10)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level >= 2;
            // Tut Collection Special
            case 13:
                if (mode != GameMode.Normal) return false;
                if (!save.Complete.Contains(tut - 1)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level >= 3;
            case 131:
                if (mode != GameMode.Normal) return false;
                if (!save.Complete.Contains(13)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level >= 3;
            case 132:
                if (mode != GameMode.Normal) return false;
                if (!save.Complete.Contains(tut - 1)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level >= 3;
            case 133:
                return _mapSave.Zone == 0 && _mapSave.Level == 3;
            // Tut trong Universe, BXH, Task Amplifier
            case 1001:
                return mode == GameMode.Universe;
            case 1002:
                return true;
            case 1003:
                return save.Complete.Contains(1004);
            case 1004:
                return mode == GameMode.Universe;
            case 1005:
                return mode == GameMode.Universe && Constant.EnableRaceEvent.Contains(remote.Universe) && DataManager.Save.raceEventNormalSave.IsTimePlayEvent;
            // Tut trong UniverseSpecial, BXH
            case 1011:
                return mode == GameMode.UniverseSpecial;
            case 1012:
                return true;
            case 1013:
                return save.Complete.Contains(1014);
            case 1014:
                return mode == GameMode.UniverseSpecial;
            case 1015:
                return mode == GameMode.UniverseSpecial && Constant.EnableRaceEvent.Contains(remote.UniverseSpecial) && DataManager.Save.raceEventSpecialSave.IsTimePlayEvent;
         
            // Tut Skill Special
            case 14:
                return save.Complete.Contains(132);
            case 15:
                return _mapSave.Zone == 0 && _mapSave.Level >= 1;
            // Tut Helper
            case 16:
                return _mapSave.Zone == 0 && _mapSave.Level >= 4;
            case 161:
                if (!save.Complete.Contains(16)) return false;
                return _mapSave.Zone == 0 && _mapSave.Level >= 4;
            case 17:
                return true;
            case 18:
                return mode == GameMode.Normal;
            case 19:
                return _mapSave.Zone == 0 && _mapSave.Level == 2;
            // Tut Minigame Merge
            case 20:
                if (mode != GameMode.Normal) return false;
                return _mapSave.Zone == 1 && _mapSave.Level == 0;
            case 21:
                return mode == GameMode.MiniGame && save.Complete.Contains(20);
            case 22:
                return mode == GameMode.MiniGame && save.Complete.Contains(21);
            // Tut Slot Machine
            case 23:
                return mode == GameMode.Universe;
            case 24:
                return save.Complete.Contains(23);
            case 25:
                return save.Complete.Contains(24);
            case 26:
                return save.Complete.Contains(25);
            case 203:
                return mode == GameMode.UniverseSpecial;
            case 204:
                return save.Complete.Contains(203);
            case 205:
                return save.Complete.Contains(204);
            case 206:
                return save.Complete.Contains(205);
            // Tut Holly jolly
            case 27:
                return mode != GameMode.Normal && remote.Universe == Universe.Noel_New;
            // Tut Event Point Normal
            case 28:
                return remote.EnablePointEvent && mode == GameMode.Normal;
            case 29:
                return save.Complete.Contains(28);
            case 30:
                return save.Complete.Contains(29);
            // Tut Event Point Special
            case 281:
                return remote.EnablePointEventSpecial && mode == GameMode.Normal;
            case 291:
                return save.Complete.Contains(281);
            case 301:
                return save.Complete.Contains(291);
            // Tut Operation
            case 33:
                if (mode != GameMode.Normal) return false;
                return _mapSave.Zone == 0 && _mapSave.Level >= 1;
            // Tut Colection Clothes
            case 34:
                if (mode != GameMode.Normal) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 6;
            // Tut Invite Friend
            case 35:
                if (mode != GameMode.Normal) return false;
                return _mapSave.Zone == 0 && _mapSave.Level == 6;
            // Tut Event Puzzle
            case 36:
                return mode == GameMode.Normal;
            case 37:
                return save.Complete.Contains(36);
        }

        return false;
    }

    public override bool? Handler(int tut, int step)
    {
        switch (tut)
        {
            case 1:
            {
                switch (step)
                {
                    case 3:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 2:
            {
                switch (step)
                {
                    case 2:
                        UI.HideHand();
                        UI.HideText();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 3:
            {
                switch (step)
                {
                    case 2:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 31:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        UI.HideText();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 32:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        UI.HideText();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 4:
            {
                switch (step)
                {
                    case 3:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 41: { switch (step) { case 1: Complete(save.CurTut); return true; } }
                break;
            case 42:
            {
                switch (step)
                {
                    case 2:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 5:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        UI.HideText();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 61: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 611:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        UI.HideText();
                        Complete(save.CurTut);
                        UI.ShowLock(LockType.Transparent);
                        return true;
                }
            }
                break;
            case 62:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        UI.HideText();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 7:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        UI.HideText();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 8:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        UI.HideText();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 9:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 91: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 92: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 93: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 94: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 95: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 10:
            {
                switch (step)
                {
                    case 2:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 101:
            {
                switch (step)
                {
                    case 3:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 102: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 11:
            {
                switch (step)
                {
                    case 2:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 12:
            {
                switch (step)
                {
                    case 2:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 13:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        return false;
                    case 3:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 131:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 132:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        return false;
                    case 2:
                        UI.HideHand();
                        return false;
                    case 3:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 133: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 1001: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 1002: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 1003: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 1004: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 1005: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 1011: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 1012: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 1013: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 1014: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 1015: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 14:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }

                break;
            }
            case 15: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 16: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 161:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        return false;
                    case 2:
                        UI.HideHand();
                        return false;
                    case 3:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 17: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 18: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 19: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 20: { switch (step) { case 1: Complete(save.CurTut); return true; } } break;
            case 21:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        return false;
                    case 2:
                        UI.HideHand();
                        return false;
                    case 3:
                        UI.HideHand();
                        UI.HideText();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 22: { switch (step) { case 1: UI.HideHand(); Complete(save.CurTut); return true; } } break;
            case 23: { switch (step) { case 1: UI.HideHand(); Complete(save.CurTut); return true;} } break;
            case 24: { switch (step) { case 1: UI.HideHand(); Complete(save.CurTut); return true; } } break;
            case 25:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 26:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 203: { switch (step) { case 1: UI.HideHand(); Complete(save.CurTut); return true;} } break;
            case 204: { switch (step) { case 1: UI.HideHand(); Complete(save.CurTut); return true; } } break;
            case 205:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 206:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 27:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        return false;
                    case 2:
                        UI.HideHand();
                        return false;
                    case 3:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 28:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        return false;
                    case 2:
                        UI.HideHand();
                        return false;
                    case 3:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 29:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        return false;
                    case 2:
                        UI.HideHand();
                        return false;
                    case 3:
                        UI.HideHand();
                        UI.HideText();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 30:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        UI.HideText();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 281:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        return false;
                    case 2:
                        UI.HideHand();
                        return false;
                    case 3:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 291:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        return false;
                    case 2:
                        UI.HideHand();
                        return false;
                    case 3:
                        UI.HideHand();
                        UI.HideText();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 301:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        UI.HideText();
                        Complete(save.CurTut);
                        return true;
                }
            }
                break;
            case 33:
            {
                switch (step)
                {
                    case 2:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            } break;
            case 34:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        return false;
                    case 2:
                        UI.HideHand();
                        return false;
                    case 3:
                        UI.HideHand();
                        UI.HideText();
                        return false;
                    case 4:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return true;
                }
            } break;
            case 35:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        return false;
                    case 2:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return false;
                }
            } break;
            case 36:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        return false;
                    case 2:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return false;
                }
            } break;
            case 37:
            {
                switch (step)
                {
                    case 1:
                        UI.HideHand();
                        return false;
                    case 2:
                        UI.HideHand();
                        Complete(save.CurTut);
                        return false;
                }
            } break;
        }

        return false;
    }
}