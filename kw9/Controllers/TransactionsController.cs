using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kw9.Data;
using kw9.Models;
using Microsoft.AspNetCore.Identity;
using kw9.Areas.Identity.Pages.Account;
using static kw9.Areas.Identity.Pages.Account.RegisterModel;

namespace kw9.Controllers
{
     public class TransactionsController : Controller
    {
        private readonly UserManager<RegisterModel.ApplicationUser> _userManager;
        private readonly SignInManager<RegisterModel.ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public TransactionsController(
            UserManager<RegisterModel.ApplicationUser> userManager,
            SignInManager<RegisterModel.ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Transactions.ToListAsync());
        }

        

        // GET: Additing/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Additing/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transactions transact, string receiverId, double sum)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserName(User);
                ApplicationUser currentUser = _context.Users.FirstOrDefault(u => u.UserName == userId);
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == receiverId);
                string transactionName = "Money Transfer";
                Transactions tr = new Transactions
                {
                    Date = DateTime.Now,
                    ReceiverId = user.UserName,
                    Balance = sum,
                    SenderId = currentUser.UserName,
                    TransactionName = transactionName
                };

                _context.Add(tr);

                currentUser.Balance -= sum;
                user.Balance += sum;

                _context.Update(currentUser);
                _context.Update(user);
                _context.SaveChanges();

            }
            return View(transact);
        }


        private bool TransactionsExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
