import React, { useEffect, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { appConfig } from '../config/AppConfig';
import { useApiGet } from '../common/api';
import { Collection } from './models';
import { Link } from 'react-router-dom';
import { FaPen, FaPlus } from 'react-icons/fa';
import { App } from '../apps/models';

interface ICollectionsProps {
  currentApp: App
}

const Collections: React.FunctionComponent<ICollectionsProps> = ({ currentApp }) => {
  const { t } = useTranslation(['Labels', 'Messages']);
  const url = `${appConfig.apiBaseUrl}/${currentApp.id}/collections`;
  const { data: collections, isLoading, doGet } = useApiGet<Collection[]>(url, []);

  // Explicitly update the collections list when the url (currentApp) changes, except on the 
  // first mount because that conflicts with how the useApiGet works.
  const isInitialMount = useRef(true);
  useEffect(() => {
    if (isInitialMount.current) {
      isInitialMount.current = false;
    } else {
       doGet(url);
    }
  }, [url]);

  return (
    <React.Fragment>
      <div className="d-flex flex-row justify-content-between align-items-center">
        <h3>{t('Labels:Collections')}</h3>
        <Link to="/collections/create" className="btn btn-light mr-3">
          <FaPlus className="mr-1" />
          {t('Labels:NewCollection')}
        </Link>    
      </div>
      <p>
        <small className="text-muted">{t('Labels:CollectionsHelpText')}</small>
      </p>
      {isLoading
        ?
          <div>{t('Messages:Loading')}</div>
        :
          <div className="d-flex flex-row flex-wrap">
            {collections.length > 0
              ? collections.map(collection => {
                return (
                  <div key={collection.id} className="w-50">
                    <div className="card mr-3 mb-3">
                      <div className="card-body">
                        <div className="d-flex flex-row justify-content-between align-items-start">
                          <h5 className="card-title">{collection.name}</h5>
                          <Link to={`/collections/edit/${collection.id}`} title={t('Labels:EditCollection')}><FaPen /></Link>
                        </div>
                        <div className="card-text">{collection.description}</div>
                        <div className="card-text mt-1 mb-3">
                          <small className="text-muted">{t('Labels:ContentType')}: {collection.contentType.name}</small>
                        </div>
                        <div>
                          <Link to={`/content/${collection.id}`} className="btn btn-primary">{t('Labels:ManageItems')}</Link>
                        </div>
                      </div>
                    </div>
                  </div>
                )
                })
              : <div>{t('Labels:NoCollectionsFound')}</div>
            }
          </div>    
      }
    </React.Fragment>
  );
};

export default Collections;
