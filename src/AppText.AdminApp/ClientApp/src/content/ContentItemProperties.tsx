import React from 'react';
import { ContentItem } from './models';
import { useTranslation } from 'react-i18next';

interface IContentItemPropertiesProps {
  contentItem: ContentItem
}

const ContentItemProperties: React.FunctionComponent<IContentItemPropertiesProps> = ({ contentItem }) => {
  const { t } = useTranslation('Labels');
  var lastUpdated = contentItem.lastModifiedAt
    ? new Date(contentItem.lastModifiedAt).toLocaleString()
    : contentItem.createdAt
      ? new Date(contentItem.createdAt).toLocaleString()
      : t('Labels:Unknown');
  return (
    <div>
      <small className="text-muted">{t('Labels:ContentItemInfo', { version: contentItem.version, lastUpdated: lastUpdated, lastModifiedBy: contentItem.lastModifiedBy || contentItem.createdBy })}</small>
    </div>
  );
};

export default ContentItemProperties;
