﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;

        public CommentController(ICommentRepository commentRepository, 
                                    IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }
        // GET: CommentController
        public ActionResult Index(int id)
        {
            List<Comment> comments = _commentRepository.GetAllCommentsFromPost(id);
            Post post = _postRepository.GetPublishedPostById(id);
            CommentViewModel cvm = new CommentViewModel
            {
                Comments = comments,
                Post = post

            };
            
            return View(cvm);
        }

        // GET: CommentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CommentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment, int id)
        {
            try
            {
                comment.PostId = id;
                comment.CreateDateTime = DateAndTime.Now;
                comment.UserProfileId = GetCurrentUserProfileId();
                _commentRepository.AddComment(comment);
                return Redirect($"/Comment/Index/{id}");
            }
            catch
            {
                return View(comment);
            }
        }

        // GET: CommentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CommentController/Delete/5
        public ActionResult Delete(int id)
        {
            Comment comment = _commentRepository.GetcommentById(id);

            return View(comment);
        }

        // POST: CommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Comment comment)
        {
            try
            {
                Comment commentForPostId = _commentRepository.GetcommentById(id);
                int postId = commentForPostId.PostId;
                _commentRepository.DeleteComment(id);
                return Redirect($"/Comment/Index/{postId}");
            }
            catch
            {
                return View(comment);
            }
        }
        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
