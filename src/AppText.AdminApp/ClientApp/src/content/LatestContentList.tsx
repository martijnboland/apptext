import React, { useRef, useEffect } from 'react';
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
  const { data: collections, isLoading: isCollectionsLoading, doGet: getCollections } = useApiGet<Collection[]>(collectionsUrl, []);

  const contentItemsUrl = `${baseUrl}/content?orderBy=LastModifiedAtDescending&first=${itemsToShow}`;
  const { data: contentItems, isLoading: isContentItemsLoading, doGet: getContentItems } = useApiGet<ContentItem[]>(contentItemsUrl, []);

  // Explicitly update the latest items list when the baseUrl (currentApp) changes, except on the 
  // first mount because that conflicts with how the useApiGet works.
  const isInitialMount = useRef(true);
  useEffect(() => {
    if (isInitialMount.current) {
      isInitialMount.current = false;
    } else {
      getCollections(collectionsUrl);
      getContentItems(contentItemsUrl);
    }
  }, [baseUrl]);

  return (
    <div>
      <h3>{t('Labels:LatestChanges')}</h3>
      <p>
        <small className="text-muted">{t('Labels:LatestContentListHelpText', { itemsToShow: itemsToShow })}</small>
      </p>
      {collections && collections.length > 0 && contentItems && contentItems.length > 0
        ?
          contentItems.map(contentItem => {
            var collection = collections.find(c => c.id === contentItem.collectionId);
            var displayText = collection.listDisplayField
              ? contentItem.content[collection.listDisplayField][currentApp.defaultLanguage]
              : '';
            var itemUrl = `/content/${collection.id}/${contentItem.contentKey}`;
            var collectionUrl = `/content/${collection.id}`;

            return (
              <div className="card mb-3" key={contentItem.id}>
                <div className="card-body">
                  <div className="card-text">
                    <div className="row">
                      <div className="col-sm-8">
                        <Link to={itemUrl}>
                          <h6>{contentItem.contentKey}</h6>
                        </Link>
                      </div>
                      <div className=" col-sm-4 text-right">
                        <small className="text-muted">Collection <strong><Link to={collectionUrl}>{collection.name}</Link></strong></small>
                      </div>
                    </div>
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
