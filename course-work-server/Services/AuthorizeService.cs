namespace course_work_server.Services
{
    public class AuthorizeService
    {
        TokenService TokenService;
        HttpRequest Request;

        public AuthorizeService(HttpRequest Request, TokenService TokenService)
        {
            this.Request = Request;
            this.TokenService = TokenService;
        }

        public bool IsAuthorize()
        {
            string RefreshToken = null;
            Request.Cookies.TryGetValue("RefreshToken", out RefreshToken);
            if (RefreshToken == null) { return false; }

            if (!TokenService.VerifyToken(RefreshToken))
            {
                return false;
            };

            return true;
        }
    }
}
