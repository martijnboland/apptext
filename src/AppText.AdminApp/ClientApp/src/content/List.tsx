import React, { useContext, useState, useEffect } from 'react';
import { RouteComponentProps } from 'react-router';
import AppContext from '../apps/AppContext';
import { appConfig } from '../config/AppConfig';
import { useApiGet } from '../common/api';
import { Collection } from '../collections/models';
import ContentLocator from './ContentLocator';
import ListHeader from './ListHeader';
import { ContentItem } from './models';
import ListItem from './ListItem';
import EditableListItem from './EditableListItem';
import { FaPlus } from 'react-icons/fa';

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
  const [ addNew, setAddNew ] = useState(false);
  const [ editItemId, setEditItemId ] = useState<string|null>(null);
  
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

  const newItem = () => {
    setAddNew(true);
    setEditItemId(null);
  }

  const editItem = (itemId: string) => {
    setAddNew(false);
    setEditItemId(itemId);
  }

  const closeEditor = () => {
    setAddNew(false);
    setEditItemId(null);
  }

  useEffect(() => {
    if (collectionId) {
      const contentUrl = `${baseUrl}/content?collectionid=${collectionId}&contentkeystartswith=${searchTerm}`;
      getContentItems(contentUrl);
    }    
  }, [collectionId, searchTerm])

  const hasMoreLanguages = activeLanguages.length !== currentApp.languages.length;

  return (
    <div>          
      <div className="d-flex flex-row justify-content-between align-items-center">
        <h2>Content</h2>
        <button type="button" className="btn btn-primary" onClick={newItem}>
          <FaPlus className="mr-1" />
          New item
        </button>    
      </div>
      <div className="d-flex flex-horizontal">
        {!isCollectionsLoading &&
          <ContentLocator collections={collections} collectionId={collectionId} onCollectionChanged={collectionChanged} onSearch={search} />
        }
      </div>
      {currentCollection &&
        <div>
          <ListHeader allLanguages={currentApp.languages} activeLanguages={activeLanguages} onLanguageAdded={languageAdded} onLanguageRemoved={languageRemoved} />
          {addNew && 
            <EditableListItem 
              isNew={true}
              collection={currentCollection} 
              contentItem={{collectionId: currentCollection.id}}
              activeLanguages={activeLanguages} 
              hasMoreLanguages={hasMoreLanguages} 
              onClose={closeEditor} 
            />
          }
          {contentItems.map(ci => 
            ci.id === editItemId
            ?
              <EditableListItem 
                isNew={false}
                key={ci.id} 
                collection={currentCollection} 
                contentItem={ci} 
                activeLanguages={activeLanguages} 
                hasMoreLanguages={hasMoreLanguages} 
                onClose={closeEditor} 
              />
            :
              <ListItem 
                key={ci.id} 
                collection={currentCollection} 
                contentItem={ci} 
                activeLanguages={activeLanguages} 
                hasMoreLanguages={hasMoreLanguages} 
                onEdit={() => editItem(ci.id)} 
              />
          )}
        </div>
      }
    </div>
  );
};

export default List;
