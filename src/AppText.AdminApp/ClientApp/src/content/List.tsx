import React, { useContext, useState, useEffect } from 'react';
import { RouteComponentProps } from 'react-router';
import AppContext from '../apps/AppContext';
import { appConfig } from '../config/AppConfig';
import { useApiGet } from '../common/api';
import { Collection } from '../collections/models';
import ContentLocator from './ContentLocator';
import ListHeader from './ListHeader';
import { ContentItem } from './models';

interface ContentRouteProps {
  collectionId?: string
}

interface ListProps  extends RouteComponentProps<ContentRouteProps> {
}

const List: React.FC<ListProps> = ({ match }) => {
  const { currentApp } = useContext(AppContext);
  const baseUrl = `${appConfig.apiBaseUrl}/${currentApp.id}`;

  const collectionsUrl = `${baseUrl}/collections`;
  const { data: collections, isLoading: isCollectionsLoading } = useApiGet<Collection[]>(collectionsUrl, []);

  const [ collectionId, setCollectionId ] = useState(match.params.collectionId);
  const [ searchTerm, setSearchTerm ] = useState('');
  const [ activeLanguages, setActiveLanguages ] = useState([ currentApp.defaultLanguage ]);
  
  const currentCollection = collections.find(c => c.id === collectionId);
  
  const { data: contentItems, isLoading: isContentItemsLoading, doGet: getContentItems } = useApiGet<ContentItem[]>(null, []);

  if (collections.length > 0 && ! collectionId) {
    setCollectionId(collections[0].id);
  }

  const collectionChanged = (collectionId: string) => {
    setCollectionId(collectionId);
  }

  const search = (searchTerm: string) => {
    setSearchTerm(searchTerm);
  }

  const languageAdded = (language: string) => {
    setActiveLanguages([ ...activeLanguages, language ]);
  }

  const languageRemoved = (language: string) => {
    setActiveLanguages(activeLanguages.filter(l => l !== language));
  }

  useEffect(() => {
    if (collectionId) {
      const contentUrl = `${baseUrl}/content?collectionid=${collectionId}&contentkeystartswith=${searchTerm}`;
      getContentItems(contentUrl);
    }    
  }, [collectionId, searchTerm])

  return (
    <div>
      <h2>Content</h2>
      <div className="d-flex flex-horizontal">
        {!isCollectionsLoading &&
          <ContentLocator collections={collections} collectionId={collectionId} onCollectionChanged={collectionChanged} onSearch={search} />
        }
      </div>
      {currentCollection &&
        <div>
          <ListHeader allLanguages={currentApp.languages} activeLanguages={activeLanguages} onLanguageAdded={languageAdded} onLanguageRemoved={languageRemoved} />
          {contentItems.map(ci => {
            const firstContentField = currentCollection.contentType && currentCollection.contentType.contentFields.length > 0
              ? currentCollection.contentType.contentFields[0].name
              : null;
            const title = currentCollection.listDisplayField && ci.content[currentCollection.listDisplayField]
              ? ci.content[currentCollection.listDisplayField][currentApp.defaultLanguage]
              : firstContentField && ci.content[firstContentField]
                ? ci.content[firstContentField][currentApp.defaultLanguage]
                : ci.contentKey
            return (
              <div className="row" key={ci.id}>
                <div className="col-3">{ci.contentKey}</div>
                <h5 className="col">{title}</h5>
              </div>
            );
          })}
        </div>
      }
    </div>
  );
};

export default List;
