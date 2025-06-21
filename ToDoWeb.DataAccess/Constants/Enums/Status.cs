namespace TodoWeb.Constants.Enums
{
    public enum Status
    {
        Unverified, // chưa xác thực email, mặc định = 0
        Verified, // đã xác thực email
        Banned, // bị khóa
        Deleted, // bị xóa,
        NotStarted, // chưa bắt đầu
        InProgress, // đang diễn ra
        Finished, // đã kết thúc
        Canceled // đã hủy
    }
}
