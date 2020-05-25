using AppText.Shared.Commands;
using AppText.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AppText.Features.ContentManagement
{
    public class SaveContentItemCommand : ICommand
    {
        public string Id { get; set; }

        public string AppId { get; set; }

        [Required]
        public string ContentKey { get; set; }

        [Required]
        public string CollectionId { get; set; }

        public Dictionary<string, object> Meta { get; set; }

        public Dictionary<string, object> Content { get; set; }

        public int Version { get; set; }

        public string[] LanguagesToValidate { get; set; }

        public SaveContentItemCommand()
        {
            LanguagesToValidate = new string[0];
            Meta = new Dictionary<string, object>();
            Content = new Dictionary<string, object>();
        }

        public ContentItem CreateContentItem(IPrincipal currentUser)
        {
            var contentItem = new ContentItem
            {
                AppId = this.AppId,
                ContentKey = this.ContentKey,
                CollectionId = this.CollectionId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Identity?.Name ?? "anonymous"
            };
            if (this.Meta != null)
            {
                contentItem.Meta = this.Meta;
            }
            if (this.Content != null)
            {
                contentItem.Content = this.Content;
            }
            return contentItem;
        }

        public void UpdateContentItem(ContentItem contentItem, IPrincipal currentUser)
        {
            contentItem.ContentKey = this.ContentKey;
            contentItem.CollectionId = this.CollectionId;
            contentItem.Version = this.Version;
            contentItem.LastModifiedAt = DateTime.UtcNow;
            contentItem.LastModifiedBy = currentUser.Identity?.Name ?? "anonymous";
            if (this.Meta != null)
            {
                contentItem.Meta = this.Meta;
            }
            if (this.Content != null)
            {
                contentItem.Content = this.Content;
            }
        }
    }

    public class SaveContentItemCommandHandler : ICommandHandler<SaveContentItemCommand>
    {
        private readonly IContentStore _store;
        private readonly IVersioner _versioner;
        private readonly ContentItemValidator _validator;
        private readonly ClaimsPrincipal _currentUser;

        public SaveContentItemCommandHandler(IContentStore store, IVersioner versioner, ContentItemValidator validator, ClaimsPrincipal currentUser)
        {
            _store = store;
            _versioner = versioner;
            _validator = validator;
            _currentUser = currentUser;
        }

        public async Task<CommandResult> Handle(SaveContentItemCommand command)
        {
            var result = new CommandResult();

            ContentItem contentItem;
            if (command.Id == null)
            {
                contentItem = command.CreateContentItem(_currentUser);
            }
            else
            {
                contentItem = await _store.GetContentItem(command.Id, command.AppId);
                if (contentItem == null)
                {
                    result.SetNotFound();
                    return result;
                }
                command.UpdateContentItem(contentItem, _currentUser);
            }

            if (! await _validator.IsValidForLanguages(contentItem, command.LanguagesToValidate))
            {
                result.AddValidationErrors(_validator.Errors);
            }
            else
            {
                if (! await _versioner.SetVersion(command.AppId, contentItem))
                {
                    result.SetVersionError();
                }
                else
                {
                    if (contentItem.Id == null)
                    {
                        await _store.AddContentItem(contentItem);
                    }
                    else
                    {
                        await _store.UpdateContentItem(contentItem);
                    }
                    result.SetResultData(contentItem);
                }
            }
            return result;
        }
    }
}
