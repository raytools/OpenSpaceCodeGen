using System.Collections.Generic;
using OpenSpaceCodeGen.AITypes;

namespace OpenSpaceCodeGen.Config {

    public enum Game
    {
        HypePC,
        LargoPC,
        TTSE,
        R2PC,
        R2DC,
        R2PS2,
        R2N64,
        R3PC,
        R3GC,
        R3PS2,
    }

    public class GameConfig
    {
        public AIType AITypes;
        public NodeSettings NodeSettings;

        public GameConfig(AIType aiTypes, NodeSettings nodeSettings)
        {
            AITypes = aiTypes;
            NodeSettings = nodeSettings;
        }

        private static Dictionary<Game, GameConfig> _mapping = new Dictionary<Game, GameConfig>()
        {
            {Game.HypePC, new GameConfig(AIType.Hype, NodeSettings.SettingsDefault)},
            {Game.LargoPC, new GameConfig(AIType.Largo, NodeSettings.SettingsDefault)},
            {Game.TTSE, new GameConfig(AIType.TTSE, NodeSettings.SettingsDefault)},
            {Game.R2PC, new GameConfig(AIType.R2, NodeSettings.SettingsDefault)},
            {Game.R2DC, new GameConfig(AIType.R2, NodeSettings.SettingsDreamcast)},
            {Game.R2PS2, new GameConfig(AIType.R2, NodeSettings.SettingsDefault)},
            {Game.R2N64, new GameConfig(AIType.R2ROM, NodeSettings.SettingsDefault)},
            {Game.R3PC, new GameConfig(AIType.R3, NodeSettings.SettingsDefault)},
            {Game.R3GC, new GameConfig(AIType.R3_GC, NodeSettings.SettingsGamecube)},
            {Game.R3PS2, new GameConfig(AIType.R3, NodeSettings.SettingsDefault)},
        };

        public static GameConfig FromGame(Game game) => _mapping[game];
    }
}
