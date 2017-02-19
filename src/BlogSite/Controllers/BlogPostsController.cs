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

        public BlogPostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

            var createViewModel = new CreateBlogPostViewModel() { BlogPost = entity, BlogCategories = listCategories };

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

            var blogPost = await _context.BlogPosts.Include(b => b.PostCategories).ThenInclude(pc => pc.BlogCategory).AsNoTracking().SingleOrDefaultAsync(m => m.ID == id);
            if (blogPost == null)
            {
                return NotFound();
            }
            

            var editViewModel = await PrepareEditViewModel(blogPost);

            if (editViewModel == null)
            {
                return NotFound();
            }
            
            return View(editViewModel);
        }

        private async Task<EditBlogPostViewModel> PrepareEditViewModel(BlogPost blogPost)
        {
            var editViewModel = new EditBlogPostViewModel() { BlogPost = blogPost };

            var blogCategories = await _context.BlogCategories.ToListAsync();

            if (blogCategories == null || !blogCategories.Any())
            {
                return null;
            }

            foreach (var blogCategory in blogCategories)
            {
                var postCategoryViewModel = new PostCategoryViewModel() { BlogCategory = blogCategory, IsChecked = blogPost.PostCategories.Any(pc => pc.BlogCategoryId == blogCategory.ID) };
                editViewModel.PostCategoryViewModels.Add(postCategoryViewModel);
            }
            
            return editViewModel;
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string[] selectedCategories)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPostToUpdate = await 
                _context.BlogPosts.Include(b => b.PostCategories)
                    .ThenInclude(pc => pc.BlogCategory)
                    .SingleOrDefaultAsync(b => b.ID == id);

            if (await TryUpdateModelAsync<BlogPost>(blogPostToUpdate, "", b => b.Article, b => b.Title))
            {
                UpdateBlogPostCategories(blogPostToUpdate, selectedCategories);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                }
                return RedirectToAction("Index");
            }

            return View(await PrepareEditViewModel(blogPostToUpdate));
        }

        private void UpdateBlogPostCategories(BlogPost blogPost, string[] selectedCategories)
        {
            if (selectedCategories == null || !selectedCategories.Any())
            {
                blogPost.PostCategories = new List<PostCategory>();
                return;
            }

            var hashSelectedCateries = new HashSet<string>(selectedCategories);
            var blogPostCategories = new HashSet<string>(blogPost.PostCategories.Select(c => c.BlogCategoryId));
            
            foreach (var contextBlogCategory in _context.BlogCategories)
            {
                if (hashSelectedCateries.Contains(contextBlogCategory.ID))
                {
                    if (!blogPostCategories.Contains(contextBlogCategory.ID))
                    {
                        blogPost.PostCategories.Add(new PostCategory(){ID = Guid.NewGuid().ToString(), BlogCategoryId = contextBlogCategory.ID,BlogPostId = blogPost.ID});
                    }
                }    
                else if (blogPostCategories.Contains(contextBlogCategory.ID))
                {
                    var catToRemove = blogPost.PostCategories.SingleOrDefault(pc => pc.BlogCategoryId == contextBlogCategory.ID);
                    _context.Remove(catToRemove);
                }
            }
            
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
            var blogPost = await _context.BlogPosts.Include(b => b.PostCategories).SingleOrDefaultAsync(m => m.ID == id);
            if (blogPost != null)
            {
                foreach (var blogPostPostCategory in blogPost.PostCategories)
                {
                    _context.PostCategories.Remove(blogPostPostCategory);
                }
            }
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
