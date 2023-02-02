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
  const deleteCollectionWithItems = useApi<Collection>(`${url}?deleteItems=true`, 'DELETE');
   
  const handleSave = (collection: Collection): Promise<any> => {
    return updateCollection.callApi(collection)
      .then(res => {
        if (res.ok) {
          toast.success(t('Messages:CollectionUpdated', { name: collection.name }));
          history.push('/');
        }
        return res;
      });
  };

  const [showDeleteConfirmation, hideDeleteConfirmation] = useModal(() => (
      <Confirm
        visible={true}
        title={t('Labels:DeleteCollection')}
        onOk={() => handleDelete(collection, false, hideDeleteConfirmation)}
        onCancel={hideDeleteConfirmation}
      >
        {t('Messages:DeleteConfirm', { name: collection.name })}
      </Confirm>
  ), [collection]);

  const [showDeleteItemsToo, hideDeleteItemsToo] = useModal(() => (
    <Confirm
      visible={true}
      title={t('Labels:DeleteCollection')}
      onOk={() => handleDelete(collection, true, hideDeleteItemsToo)}
      onCancel={hideDeleteItemsToo}
    >
      {t('Messages:DeleteItemsTooConfirm')}
    </Confirm>
), [collection]);

  const handleDelete = (collection, deleteItemsToo: boolean, hideConfirmation): Promise<any> => {
    const deleteMutation = deleteItemsToo ? deleteCollectionWithItems : deleteCollection;
    return deleteMutation.callApi(collection)
      .then(res => {
        if (res.ok) {
          toast.success(t('Messages:CollectionDeleted', { name: collection.name }));
          history.push('/');
        }
        else {
          if (res.apiErrors?.length > 0 && res.apiErrors[0].errorMessage === 'DeleteCollectionContainsContent') {
            hideConfirmation();
            showDeleteItemsToo();
          } else {
            toast.error(Object.values(res.errors).join(','));
          }
        }
        return res;
      })
      .catch(err => {
        hideConfirmation();
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