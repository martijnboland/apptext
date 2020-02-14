import React from 'react';
import { Collection } from '../collections/models';
import { ContentItem } from './models';
import { FaPen } from 'react-icons/fa';

interface IListItemProps {
  collection: Collection,
  contentItem: ContentItem,
  activeLanguages: string[],
  hasMoreLanguages: boolean, 
  onEdit: () => void
}

const ListItem: React.FunctionComponent<IListItemProps> = ({ collection, contentItem, activeLanguages, hasMoreLanguages, onEdit }) => {

  const firstContentField = collection.contentType && collection.contentType.contentFields.length > 0
    ? collection.contentType.contentFields[0].name
    : null;
  
  const titleItems = activeLanguages.map(lang => {
    const title = collection.listDisplayField && contentItem.content[collection.listDisplayField]
      ? contentItem.content[collection.listDisplayField][lang]
      : firstContentField && contentItem.content[firstContentField]
        ? contentItem.content[firstContentField][lang]
        : contentItem.contentKey
    return { lang: lang, title: title};
  });
  

  return (
    <div className="row align-items-center border-bottom" key={contentItem.id}>
      <div className="col-3">{contentItem.contentKey}</div>
      {titleItems.map(item =>
        <h5 className="col" key={item.lang}>{item.title}</h5>
      )}
      {hasMoreLanguages && 
        <div className="col"></div>
      }
      <div className="col-1">
        <button className="btn btn-link" onClick={onEdit}>
          <FaPen />
        </button>
    </div>
    </div>
  );
};

export default ListItem;
