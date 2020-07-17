import React from 'react';
import { useTranslation } from 'react-i18next';
import { App } from '../apps/models';
import { appConfig } from '../config/AppConfig';
import { useApiGet } from '../common/api';
import { Collection } from '../collections/models';
import { ContentItem } from './models';
import ContentItemProperties from './ContentItemProperties';
import { Link } from 'react-router-dom';

interface ILatestContentListProps {
  itemsToShow: number,
  currentApp: App
}

const LatestContentList: React.FunctionComponent<ILatestContentListProps> = ({ itemsToShow, currentApp }) => {
  const { t } = useTranslation('Labels');
  const baseUrl = `${appConfig.apiBaseUrl}/${currentApp.id}`;

  const collectionsUrl = `${baseUrl}/collections`;
  const { data: collections, isLoading: isCollectionsLoading } = useApiGet<Collection[]>(collectionsUrl, []);

  const contentItemsUrl = `${baseUrl}/content?orderBy=LastModifiedAtDescending&first=${itemsToShow}`;
  const { data: contentItems, isLoading: isContentItemsLoading } = useApiGet<ContentItem[]>(contentItemsUrl, []);

  return (
    <div>
      <h3>{t('Labels:LatestChanges')}</h3>
      <p>
        <small className="text-muted"></small>
      </p>
      {collections && contentItems && contentItems.length > 0
      ?
      contentItems.map(contentItem => {
        var collection = collections.find(c => c.id === contentItem.collectionId);
        var displayText = collection.listDisplayField
          ? contentItem.content[collection.listDisplayField][currentApp.defaultLanguage]
          : '';
        var itemUrl = `/content/${collection.id}/${contentItem.contentKey}`;
        return (
          <div className="card mb-3" key={contentItem.id}>
            <div className="card-body">
              <div className="card-text">
                <div className="text-right">
                  <small className="text-muted">Collection <strong>{collection.name}</strong></small>
                </div>
                <Link to={itemUrl}>
                  <h5>{contentItem.contentKey}</h5>
                </Link>
                <p>{displayText}</p>
                <ContentItemProperties contentItem={contentItem} />
              </div>
            </div>
          </div>
        )
      })
      :
        <div>{t('Labels:NoItemsFound')}</div>
      }
    </div>
  );
};

export default LatestContentList;
