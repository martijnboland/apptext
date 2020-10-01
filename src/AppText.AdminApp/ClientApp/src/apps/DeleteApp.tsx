import React from 'react';
import { useTranslation } from 'react-i18next';
import { FaTrash } from 'react-icons/fa';
import { useModal } from 'react-modal-hook';
import { toast } from 'react-toastify';
import { Collection } from '../collections/models';
import { useApi, useApiGet } from '../common/api';
import Confirm from '../common/components/dialogs/Confirm';
import { appConfig } from '../config/AppConfig';
import { App } from './models';

interface IDeleteAppProps {
  app: App,
  onAppDeleted(): void
}

const DeleteApp: React.FunctionComponent<IDeleteAppProps> = ({ app, onAppDeleted }) => {
  const { t } = useTranslation(['Labels', 'Messages']);

  const url = `${appConfig.apiBaseUrl}/apps/${app.id}`;
  const deleteApp = useApi<void>(url, 'DELETE');

  const collectionsUrl = `${appConfig.apiBaseUrl}/${app.id}/collections`;
  const { data: collections } = useApiGet<Collection[]>(collectionsUrl, []);

  const [showDeleteConfirmation, hideDeleteConfirmation] = useModal(() => (
    <Confirm
      visible={true}
      title={t('Labels:DeleteApp')}
      onOk={() => handleDelete(app, hideDeleteConfirmation)}
      onCancel={hideDeleteConfirmation}
    >
      {t('Messages:DeleteConfirm', { name: app.id })}
    </Confirm>
  ), [app]);

  const handleDelete = (app, hideDeleteConfirmation): Promise<any> => {
    return deleteApp.callApi()
      .then(res => {
        if (res.ok) {
          toast.success(t('Messages:AppDeleted', { name: app }));
          onAppDeleted();
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
    <React.Fragment>
      <h2>{t('Labels:DeleteApp')}</h2>
      <p>
        <small className="text-muted">{t('Labels:DeleteAppHelpText')}</small>
      </p>
      {collections.length === 0
        ?
          <div className="d-flex flex-row justify-content-between align-items-center">
            <p className="text-danger">{t('Labels:DeleteAppWarning')}</p>
            <button type="button" className="btn btn-danger" onClick={showDeleteConfirmation}>
              <FaTrash className="mr-1" />
              {t('Labels:DeleteButton')}
            </button>    
          </div>
        :
          <p>{t('Labels:DeleteAppDisabledBecauseCollections')}</p>
      }
    </React.Fragment>
  );
};

export default DeleteApp;
