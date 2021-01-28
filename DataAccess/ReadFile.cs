// Joe
// 周漫
// 2021012621:52

namespace DataAccess {
    public static class ReadFile {
        public static string[] ReadData(string filePath) {
            var dataPoints = int.Parse(IniHelper.Read("Data", "save_interval", "0", filePath));
            var temp = new string[dataPoints];
            for (var i = 0; i < dataPoints; i++) temp[i] = IniHelper.Read("Data", i.ToString(), "", filePath);
            return temp;
        }
    }
}