namespace DBP_team
{
    public static class AppSession
    {
        // 현재 로그인된 사용자 정보를 보관
        public static Models.User CurrentUser { get; set; }
    }
}
