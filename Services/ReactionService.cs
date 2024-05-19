using CourseProject_backend.Entities;
using CourseProject_backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject_backend.Services
{
    public class ReactionService
    {
        private readonly ReactionRepository _reactionRepository;

        public ReactionService
        (
            [FromServices] ReactionRepository reactionRepository   
        )
        {
            _reactionRepository = reactionRepository;
        }

        public async Task<bool> AddReaction(PositiveReaction reaction)
        {
            PositiveReaction? reactionByDb = (await _reactionRepository
                .GetValues(x => x.Id == reaction.Id)).FirstOrDefault();

            if(reactionByDb != null ) { return false; }

            await _reactionRepository.Add(reaction);

            await _reactionRepository.SaveUpdates();

            return true;
        }

        public async Task<PositiveReaction?> GetByUserAndItem(User user, Item item)
        {
            PositiveReaction? reaction = (await _reactionRepository
                .GetValues(x => x.User.Id == user.Id && x.Item.Id == item.Id)).FirstOrDefault();

            if (reaction == null) { return null; }

            return reaction;
        }

        public async Task<bool> DeleteByUserAndItem(User user, Item item)
        {
            PositiveReaction? reaction = (await _reactionRepository
                .GetValues(x => x.User.Id == user.Id && x.Item.Id == item.Id)).FirstOrDefault();

            if (reaction == null) { return false; }

            await _reactionRepository.Delete(reaction);

            await _reactionRepository.SaveUpdates();

            return true;
        }
    }
}
