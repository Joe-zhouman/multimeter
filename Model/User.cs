// Joe
// 周漫
// 2021012611:05

namespace Model {
    public class User {
        public User() {
            Name = "";
            Password = "";
            Type = UserType.NORMAL;
        }

        public string Name { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }
    }
}