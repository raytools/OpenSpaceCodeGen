namespace OpenSpaceCodeGen {
    public class NodeSettings
    {
        public byte sizeOfNode;
        public byte indexOfParam;
        public byte indexOfIndent;
        public byte indexOfType;

        public static NodeSettings SettingsDefault = new NodeSettings()
        {
            sizeOfNode = 8,
            indexOfParam = 0,
            indexOfIndent = 6,
            indexOfType = 7,
        };

        public static NodeSettings SettingsDreamcast = new NodeSettings()
        {
            sizeOfNode = 12,
            indexOfParam = 0,
            indexOfIndent = 10,
            indexOfType = 11,
        };

        public static NodeSettings SettingsGamecube = new NodeSettings()
        {
            sizeOfNode = 12,
            indexOfParam = 0,
            indexOfIndent = 10,
            indexOfType = 7,
        };
    }
}
