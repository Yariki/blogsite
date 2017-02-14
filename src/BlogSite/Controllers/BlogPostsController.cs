using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogSite.Data;
using BlogSite.Models;
using BlogSite.Models.BlogPostViewModels;
using BlogSite.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogSite.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogPostsController(ApplicationDbContext context,UserManager<ApplicationUser> userManager )
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: BlogPosts
        public async Task<IActionResult> Index()
        {
            string currentUserID = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(currentUserID))
            {
                return NotFound();
            }
            var applicationDbContext = _context.BlogPosts.Where(b => b.ApplicationUserId == currentUserID).Include(b => b.ApplicationUser).AsNoTracking();
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BlogPosts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts.Include(b => b.PostCategories).ThenInclude(c => c.BlogCategory).SingleOrDefaultAsync(m => m.ID == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Create
        public async Task<IActionResult> Create()
        {
            var userId = _userManager.GetUserId(User);
            var entity = ApplicationDbContext.CreateEntity<BlogPost>();
            entity.ApplicationUserId = userId;
            var listCategories = await _context.BlogCategories.Where(c => c.Enabled).AsNoTracking().ToListAsync();
            
            var  createViewModel = new CreateBlogPostViewModel() {BlogPost =  entity, BlogCategories = listCategories};

            return View(createViewModel);
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApplicationUserId,Article,Title")] BlogPost blogPost, string[] selectedCategories)
        {
            if (ModelState.IsValid)
            {
                blogPost.ID = Guid.NewGuid().ToString();
                _context.Add(blogPost);
                if (selectedCategories != null && selectedCategories.Length > 0)
                {
                    blogPost.PostCategories = new List<PostCategory>();
                    foreach (var selectedCategory in selectedCategories)
                    {
                        var postCategory = ApplicationDbContext.CreateEntity<PostCategory>();
                        postCategory.BlogPostId = blogPost.ID;
                        postCategory.BlogCategoryId = selectedCategory;
                        blogPost.PostCategories.Add(postCategory);
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", blogPost.ApplicationUserId);
            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts.SingleOrDefaultAsync(m => m.ID == id);
            if (blogPost == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", blogPost.ApplicationUserId);
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,ApplicationUserId,Article,Created,Modified,Title,Views")] BlogPost blogPost)
        {
            if (id != blogPost.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPost.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", blogPost.ApplicationUserId);
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts.SingleOrDefaultAsync(m => m.ID == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var blogPost = await _context.BlogPosts.SingleOrDefaultAsync(m => m.ID == id);
            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool BlogPostExists(string id)
        {
            return _context.BlogPosts.Any(e => e.ID == id);
        }
    }
}
