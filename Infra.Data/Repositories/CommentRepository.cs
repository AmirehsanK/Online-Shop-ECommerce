using Domain.Entities.Comment;
using Domain.Interface;
using Domain.ViewModel.Comment;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;


namespace Infra.Data.Repositories
{
    public class CommentRepository(ApplicationDbContext context) : ICommentRepository
    {
        public async Task<List<Comment>> GetAllPendingCommentsAsync()
        {
            return await context.Comments.Where(c => !c.IsApproved && !c.IsDeleted).ToListAsync();
        }
        public async Task<FilterCommentViewModel> GetAllCommentsAsync(FilterCommentViewModel filter)
        {
            var query = context.Comments.Include(p => p.Ratings).Where(p => !p.IsDeleted).AsQueryable();
            var filteredComments = filter.Filter switch
            {
                "approved" => context.Comments.Where(c => c.IsApproved),
                "notApproved" => context.Comments.Where(c => !c.IsApproved),
                _ => context.Comments // "all"
            };
            await filter.Paging(query.Select(p => new CommentViewModel()
            {
                Content = p.Content
                ,Id = p.Id
                ,IsApproved = p.IsApproved
                ,ProductId = p.ProductId
                ,Title = p.Title
                ,Strengths = p.Strengths
                ,Weaknesses = p.Weaknesses
                ,Ratings = (List<CommentRatingViewModel>)p.Ratings,
                CreateDate = p.CreateDate
            }));
            return filter;
        }

        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            return await context.Comments.Include(c => c.Ratings)
                .Include(c => c.Interactions).Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddCommentAsync(Comment comment)
        {
            context.Comments.Add(comment);
            await context.SaveChangesAsync();
        }

        public async Task ApproveCommentAsync(int id)
        {
            var comment = await context.Comments.FindAsync(id);
            if (comment != null)
            {
                comment.IsApproved = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task LikeCommentAsync(int commentId, string userIp, bool isLike)
        {
            var existingInteraction = await context.UserInteraction
                .FirstOrDefaultAsync(ui => ui.CommentId == commentId && ui.UserIp == userIp);

            if (existingInteraction == null)
            {
                var interaction = new UserInteraction
                {
                    CommentId = commentId,
                    UserIp = userIp,
                    IsLike = isLike,
                    Comment = null
                };
                context.UserInteraction.Add(interaction);
            }
            else
            {
                existingInteraction.IsLike = isLike;
            }

            await context.SaveChangesAsync();
        }

        public async Task DeleteComment(Comment comment)
        {
            context.Comments.Update(comment);

            await context.SaveChangesAsync();
        }
    }
}
