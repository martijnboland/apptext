import React, { useContext } from 'react';

import CollectionForm from './CollectionForm';
import { RouteComponentProps } from 'react-router';
import AppContext from '../apps/AppContext';
import { appConfig } from '../config/AppConfig';
import { useApi, useApiGet } from '../common/api';
import { Collection } from './models';
import { toast } from 'react-toastify';
import { ContentType } from '../contenttypes/models';

const Create: React.FC<RouteComponentProps> = ({ history }) => {

  const { currentApp } = useContext(AppContext);
  const url = `${appConfig.apiBaseUrl}/${currentApp.id}/collections`;
  const createCollection = useApi<Collection>(url, 'POST');

  const contentTypesUrl = `${appConfig.apiBaseUrl}/${currentApp.id}/contenttypes`;
  const { data: contentTypes } = useApiGet<ContentType[]>(contentTypesUrl, []);

  const initialCollection: Collection = {
    id: undefined,
    name: undefined,
    contentType: contentTypes.length > 0 ? contentTypes[0]: undefined
  }
   
  const handleSave = (collection: Collection): Promise<any> => {
    return createCollection.callApi(collection)
      .then(res => {
        if (res.ok) {
          toast.success(`Collection ${collection.name} created`);
          history.push('/collections');
        }
        return res;
      })
  };

  return (
    <>
      <h2>Create collection</h2>
      {contentTypes &&
        <div className="row">
          <div className="col-lg-8">
            <CollectionForm collection={initialCollection} contentTypes={contentTypes} onSave={handleSave} />
          </div>
          <div className="col-4">
          </div>        
        </div>    
      }
    </>
  );
};

export default Create;