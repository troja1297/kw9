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
    public class AdditingController : Controller
    {
        private readonly UserManager<RegisterModel.ApplicationUser> _userManager;
        private readonly SignInManager<RegisterModel.ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AdditingController(
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
        public async Task<IActionResult> Create(Transactions transactions, double balance)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.Id == userId);
                user.Balance += balance;
                string transactionName = "Money Load";
                Transactions transaction = new Transactions { Date = DateTime.Now, ReceiverId = user.UserName, Balance = balance, SenderId = "load balance", TransactionName = transactionName };
                _context.Add(transaction);
                _context.Update(user);
                _context.SaveChanges();
            }
            return View(transactions);
        }


        private bool TransactionsExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
