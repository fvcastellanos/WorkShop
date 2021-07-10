namespace WorkShop.Clients.Domain
{
    public class LoginResponse
    {
        public string Jwt { get; set; }
        public User User{ get; set; }
    }
}
