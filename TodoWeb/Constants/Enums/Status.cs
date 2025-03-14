namespace TodoWeb.Constants.Enums
{
    public enum Status
    {
        Unverified, // chưa xác thực email, mặc định = 0
        Verified, // đã xác thực email
        Banned, // bị khóa
        Deleted // bị xóa
    }
}
