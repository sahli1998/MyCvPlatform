using MonCv.Model;

namespace MonCv.IRepositories
{
    public interface IRepoAuth
    {
        public AuthModel Register(RegisterModel model);
        public AuthModel Login(LoginModel model);
    }
}
