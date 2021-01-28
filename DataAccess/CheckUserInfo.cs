// Joe
// 周漫
// 2021012611:03

using Model;

namespace DataAccess {
    public static class CheckUserInfo {
        public static bool CheckNormalUser(User user) {
            return true;
        }

        public static bool CheckAdvanceUser(User user) {
            return user.Name.Equals("admin") && user.Password.Equals("admin");
        }
    }
}