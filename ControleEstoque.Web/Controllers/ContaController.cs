using ControleEstoque.Web.Models;
using Microsoft.Ajax.Utilities;
using System.Web.Mvc;
using System.Web.Security;

namespace ControleEstoque.Web.Controllers
{
    public class ContaController : Controller
    {

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel login, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            // vai ate o model e executa as classes passando como paramentro usuario e senha que veio no post da pagina
            var usuario = UsuarioModel.ValidaUsuario(login.Usuario, login.Senha);               

            if (usuario != null)
            {
                FormsAuthentication.SetAuthCookie(usuario.Nome, login.LembrarMe);
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            else
            {
                ModelState.AddModelError("", "Login inválido");
            }

            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


    }
}