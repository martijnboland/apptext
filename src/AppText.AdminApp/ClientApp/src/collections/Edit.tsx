import React, { useContext } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { useModal } from 'react-modal-hook';
import { toast } from 'react-toastify';

import AppContext from '../apps/AppContext';
import { appConfig } from '../config/AppConfig';
import { useApiGet, useApi } from '../common/api';
import { Collection } from './models';
import { IdParams } from '../common/routeParams';
import CollectionForm from './CollectionForm';
import Confirm from '../common/components/dialogs/Confirm';
import { ContentType } from '../contenttypes/models';
import { useTranslation } from 'react-i18next';

interface EditProps extends RouteComponentProps<IdParams> {
}

const Edit: React.FC<EditProps> = ({ match, history }) => {
  const { t } = useTranslation(['Labels', 'Messages']);
  const { currentApp } = useContext(AppContext);
  const url = `${appConfig.apiBaseUrl}/${currentApp.id}/collections/${match.params.id}`;
  const { data: collection } = useApiGet<Collection>(url);
  const contentTypesUrl = `${appConfig.apiBaseUrl}/${currentApp.id}/contenttypes?includeglobalcontenttypes=true`;
  const { data: contentTypes } = useApiGet<ContentType[]>(contentTypesUrl);
  const updateCollection = useApi<Collection>(url, 'PUT');
  const deleteCollection = useApi<Collection>(url, 'DELETE');
   
  const handleSave = (collection: Collection): Promise<any> => {
    return updateCollection.callApi(collection)
      .then(res => {
        if (res.ok) {
          toast.success(t('Messages:CollectionUpdated', { name: collection.name }));
          history.push('/collections');
        }
        return res;
      });
  };

  const [showDeleteConfirmation, hideDeleteConfirmation] = useModal(() => (
      <Confirm
        visible={true}
        title={t('Labels:DeleteCollection')}
        onOk={() => handleDelete(collection, hideDeleteConfirmation)}
        onCancel={hideDeleteConfirmation}
      >
        {t('Messages:DeleteConfirm', { name: collection.name })}
      </Confirm>
  ), [collection]);

  const handleDelete = (collection, hideDeleteConfirmation): Promise<any> => {
    return deleteCollection.callApi(collection)
      .then(res => {
        if (res.ok) {
          toast.success(t('Messages:CollectionDeleted', { name: collection.name }));
          history.push('/collections');
        }
        else {
          toast.error(Object.values(res.errors).join(','));
        }
        return res;
      })
      .catch(err => {
        hideDeleteConfirmation();
      })
  }

  return (  
    <>
      <h2>{t('Labels:EditCollection')}</h2>
      {collection && contentTypes &&
        <div className="row">
          <div className="col-lg-8">
            <CollectionForm collection={collection} contentTypes={contentTypes} onSave={handleSave} onDelete={showDeleteConfirmation} />
          </div>
          <div className="col-4">
          </div>        
        </div>
      }
    </>  
  );
};

export default Edit;