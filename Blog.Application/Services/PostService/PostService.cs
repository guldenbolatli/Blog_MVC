using AutoMapper;
using Blog.Application.Models.DTOs;
using Blog.Application.Models.VMs;
using Blog.Domain.Entities.Concrete;
using Blog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepository,
                           IGenreRepository genreRepository,
                           IMapper mapper)
        {
            _postRepository=postRepository;
            _genreRepository=genreRepository;
            _mapper=mapper;
        }

        public async Task CreatePostAsync(CreatePostDTO post)
        {
            Post newPost = new Post();
            _mapper.Map(post, newPost);
          
            await _postRepository.AddAsync(newPost);
        }

        public async Task<CreatePostDTO> CreatePostAsync()
        {
            CreatePostDTO newPost = new CreatePostDTO();

            List<GenreDTO> genres = new List<GenreDTO>();

            var comingFromDbGenres = _genreRepository.GetAll().ToList();

            //Test
            //await Console.Out.WriteLineAsync("Adet=" +       comingFromDbGenres.Count.ToString());

            _mapper.Map(comingFromDbGenres,genres);
            newPost.Genres = genres;

            //await Console.Out.WriteLineAsync("Adet 2=" + newPost.Genres.Count.ToString());


            return newPost;
        }

        
        public async Task DeletePostAsync(int postId)
        {
            var post  =await _postRepository.GetFirstOrDefaultAsync(x => x.Id == postId);
            if (post != null)
            {
                await _postRepository.DeleteAsync(post);
            }
        }

        public async Task<List<PostVM>> GetAllPostsAsync()
        {
            //select
            //where
            //orderBy
            //include(JOIN)

            var allPosts=await _postRepository.GetFilteredListAsync(
                x => x,
                x => x.Status != Domain.Enums.Status.Passive,
                x => x.OrderByDescending(x => x.CreateDate),
                x => x.Include(x => x.AppUser).Include(x => x.Genre));

            List<PostVM> allPostsVM = new List<PostVM>();
            _mapper.Map(allPosts, allPostsVM);
            return allPostsVM;

        }

        public Task<PostVM> GetPostAsync(int postID)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdatePostAsync(UpdatePostDTO post)
        {
            throw new NotImplementedException();
        }
    }
}
