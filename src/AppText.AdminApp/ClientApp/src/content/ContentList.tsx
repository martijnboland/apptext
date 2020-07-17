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
import { useTranslation } from 'react-i18next';
import { appTextAdminAppId } from '../config/constants';

interface ContentListRouteProps {
  collectionId?: string,
  contentKey?: string
}

interface ContentListProps  extends RouteComponentProps<ContentListRouteProps> {
}

const ContentList: React.FC<ContentListProps> = ({ match, history }) => {
  const { t, i18n } = useTranslation('Labels');
  const { currentApp } = useContext(AppContext);
  const baseUrl = `${appConfig.apiBaseUrl}/${currentApp.id}`;

  const collectionsUrl = `${baseUrl}/collections`;
  const { data: collections, isLoading: isCollectionsLoading } = useApiGet<Collection[]>(collectionsUrl, []);

  const [ collectionId, setCollectionId ] = useState(match.params.collectionId);
  const [ searchTerm, setSearchTerm ] = useState(match.params.contentKey || '');
  const [ activeLanguages, setActiveLanguages ] = useState([ currentApp.defaultLanguage ]);
  const [ addNew, setAddNew ] = useState(false);
  const [ editItemId, setEditItemId ] = useState<string|null>(null);
  
  const currentCollection = collections.find(c => c.id === collectionId);
  const { data: contentItems, isLoading: isContentItemsLoading, doGet: getContentItems } = useApiGet<ContentItem[]>(null, []);
  
  let initialContentItem: ContentItem = undefined;
  let emptyLocalizableContent = {};
  activeLanguages.forEach(l => emptyLocalizableContent[l] = undefined);

  if (currentCollection) {
    let initialContent = {}
    currentCollection.contentType.contentFields.forEach(f => {
      initialContent[f.name] = f.isLocalizable 
        ? emptyLocalizableContent
        : undefined;
    });

    initialContentItem = {
      id: undefined,
      collectionId: currentCollection.id,
      appId: currentCollection.contentType.appId,
      contentKey: undefined,
      content: initialContent,
      meta: undefined
    }
  }

  if (collections.length > 0 && ! collectionId) {
    setCollectionId(collections[0].id);
  }

  const collectionChanged = (collectionId: string) => {
    setAddNew(false);
    setCollectionId(collectionId);
    history.replace({ pathname: `/content/${collectionId}`})
  }

  const search = (searchTerm: string) => {
    setAddNew(false);
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

  const itemChanged = () => {
    // Check if we're editing our own resources. In that case, the resources need to be reloaded
    if (currentApp.id === appTextAdminAppId) {
      i18n.reloadResources();
    }
    refreshList();
  }

  const refreshList = () => {
    setAddNew(false);
    setEditItemId(null);
    if (collectionId) {
      const contentUrl = `${baseUrl}/content?collectionid=${collectionId}&contentkeystartswith=${searchTerm}`;
      getContentItems(contentUrl, true);
    }    
  }

  useEffect(() => {
    if (collectionId) {
      const contentUrl = `${baseUrl}/content?collectionid=${collectionId}&contentkeystartswith=${searchTerm}`;
      getContentItems(contentUrl);
    }    
  }, [collectionId, searchTerm])

  return (
    <div>          
      <div className="d-flex flex-row justify-content-between align-items-center">
        <h2>{t('Labels:Content')}</h2>
        <button type="button" className="btn btn-primary" onClick={newItem}>
          <FaPlus className="mr-1" />
          {t('Labels:NewContentItem')}
        </button>    
      </div>
      <p>
        <small className="text-muted">{t('Labels:ContentListHelpText')}</small>
      </p>
      <div className="d-flex flex-horizontal">
        {!isCollectionsLoading &&
          <ContentLocator collections={collections} collectionId={collectionId} searchTerm={searchTerm} onCollectionChanged={collectionChanged} onSearch={search} />
        }
      </div>
      {currentCollection &&
        <div>
          <ListHeader allLanguages={currentApp.languages} activeLanguages={activeLanguages} onLanguageAdded={languageAdded} onLanguageRemoved={languageRemoved} />
          {addNew && 
            <EditableListItem 
              isNew={true}
              collection={currentCollection} 
              contentItem={initialContentItem}
              activeLanguages={activeLanguages} 
              onClose={closeEditor}
              onItemSaved={refreshList}
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
                onClose={closeEditor}
                onItemSaved={itemChanged}
                onItemDeleted={itemChanged}
              />
            :
              <ListItem 
                key={ci.id} 
                collection={currentCollection} 
                contentItem={ci} 
                activeLanguages={activeLanguages} 
                onEdit={() => editItem(ci.id)} 
              />
          )}
        </div>
      }
    </div>
  );
};

export default ContentList;
