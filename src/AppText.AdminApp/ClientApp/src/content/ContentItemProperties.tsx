import React from 'react';
import { ContentItem } from './models';

interface IContentItemPropertiesProps {
  contentItem: ContentItem
}

const ContentItemProperties: React.FunctionComponent<IContentItemPropertiesProps> = ({ contentItem }) => {
  var lastUpdated = contentItem.lastModifiedAt
    ? new Date(contentItem.lastModifiedAt).toLocaleString()
    : contentItem.createdAt
      ? new Date(contentItem.createdAt).toLocaleString()
      : 'unknown'
  return (
    <div>
      <small className="text-muted">Version {contentItem.version}, last update on {lastUpdated} by {contentItem.lastModifiedBy || contentItem.createdBy}</small>
    </div>
  );
};

export default ContentItemProperties;
