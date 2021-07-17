﻿using Flow.Core.Contracts;
using Flow.Core.DomainModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flow.Infrastructure.DataAccess.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IMongoCollection<Post> _database;

        public PostRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("FlowDb");
            _database = database.GetCollection<Post>("Posts");
        }

        public async Task UpsertPost(Post post)
        {
            await _database.InsertOneAsync(post);

            //BsonBinaryData binGuid = new BsonBinaryData(post.PostId, GuidRepresentation.Standard);
            
            /*await _database.ReplaceOneAsync(
                //new BsonDocument("_id", binGuid),
                p => p.PostId == post.PostId,
                post,
                new ReplaceOptions { IsUpsert = false });*/

            //var filter = Builders<Post>.Filter.Where(p => p.PostId == post.PostId);
            //var options = new FindOneAndReplaceOptions<Post, Post> { IsUpsert = true };
            //await _database.FindOneAndReplaceAsync(filter, post, options);
        }

        public async Task<ICollection<Post>> GetPostByUserId(Guid userId)
        {
            return await GetPostByUserId(userId, 0, 0);
        }

        public async Task<ICollection<Post>> GetPostByUserId(Guid userId, int skip, int take)
        {
            if (skip < 0 || take < 0)
            {
                throw new ArgumentOutOfRangeException("Error: attemtping to skip or retreive a negative number of posts.");
            }
            else
            {
                return await _database.Find(post => post.PostOwnerId == userId).Skip(skip).Limit(take).ToListAsync();
            }
        }
    }
}
