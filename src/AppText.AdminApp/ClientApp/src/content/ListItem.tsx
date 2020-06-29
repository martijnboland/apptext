import React from 'react';
import { Collection } from '../collections/models';
import { ContentItem } from './models';
import { FaPen } from 'react-icons/fa';
import ContentItemProperties from './ContentItemProperties';

interface IListItemProps {
  collection: Collection,
  contentItem: ContentItem,
  activeLanguages: string[],
  onEdit: () => void
}

const ListItem: React.FunctionComponent<IListItemProps> = ({ collection, contentItem, activeLanguages, onEdit }) => {

  const firstContentField = collection.contentType && collection.contentType.contentFields.length > 0
    ? collection.contentType.contentFields[0].name
    : null;
  
  const titleItems = activeLanguages.map(lang => {
    const title = collection.listDisplayField && contentItem.content[collection.listDisplayField]
      ? contentItem.content[collection.listDisplayField][lang]
      : firstContentField && contentItem.content[firstContentField]
        ? contentItem.content[firstContentField][lang]
        : ''
    return { lang: lang, title: title};
  });
  

  return (
    <div className="card mb-3">
      <div className="card-body">
        <div className="row align-items-center" key={contentItem.id}>
          <div className="col-3">
            {contentItem.contentKey}
          </div>
          {titleItems.map(item =>
            <h5 className="col" key={item.lang}>{item.title}</h5>
          )}
          <div className="col-2">
            <button className="btn btn-link" onClick={onEdit}>
              <FaPen />
            </button>
          </div>
        </div>
        <div className="row">
          <div className="col">
            <ContentItemProperties contentItem={contentItem} />
          </div>
        </div>
      </div>
    </div>
  );
};

export default ListItem;
