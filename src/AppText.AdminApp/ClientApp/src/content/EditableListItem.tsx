import React, { useContext } from 'react';
import { Collection } from '../collections/models';
import { ContentItem, ContentItemCommand } from './models';
import { Formik, Field, FormikHelpers } from 'formik';
import { TextInput } from '../common/components/form';
import AppContext from '../apps/AppContext';
import { appConfig } from '../config/AppConfig';
import { useApi } from '../common/api';
import { toast } from 'react-toastify';
import { FaSave, FaTimes, FaTrash } from 'react-icons/fa';
import { useModal } from 'react-modal-hook';
import Confirm from '../common/components/dialogs/Confirm';
import { editorMap } from './contentEditors';
import { useTranslation } from 'react-i18next';

interface IEditableListItemProps {
  isNew: boolean,
  collection: Collection,
  contentItem?: ContentItem,
  activeLanguages: string[],
  onClose: () => void,
  onItemSaved: () => void,
  onItemDeleted?: () => void
}

const EditableListItem: React.FunctionComponent<IEditableListItemProps> = ({ isNew, collection, contentItem, activeLanguages, onClose, onItemSaved, onItemDeleted }) => {

  const { t } = useTranslation(['Labels', 'Messages']);
  const { currentApp } = useContext(AppContext);
  const url = `${appConfig.apiBaseUrl}/${currentApp.id}/content`;
  const contentItemApiAction = isNew 
    ? useApi<ContentItemCommand>(url, 'POST')
    : useApi<ContentItemCommand>(url + `/${contentItem.id}`, 'PUT');
  const deleteContentItem = useApi<{}>(url + `/${contentItem.id}`, 'DELETE')

  const onSubmit = (contentItem: ContentItem, actions: FormikHelpers<ContentItem>): Promise<any> => {
    const contentItemCommand: ContentItemCommand = { ...contentItem, languagesToValidate: activeLanguages };
    return contentItemApiAction.callApi(contentItemCommand)
      .then(res => {
        if (res.ok) {
          if (isNew) {
            toast.success(t('Messages:ContentItemCreated', { contentKey: contentItem.contentKey}));
          } else {
            toast.success(t('Messages:ContentItemUpdated', { contentKey: contentItem.contentKey}));
          }
          onItemSaved();
        } else {
          actions.setErrors(res.errors);
        }
        return res;
      });
  };

  const handleDelete = (contentItem: ContentItem, hideDeleteConfirmation): Promise<any> => {
    return deleteContentItem.callApi(null)
      .then(res => {
        if (res.ok) {
          toast.success(t('Messages:ContentItemDeleted', { contentKey: contentItem.contentKey}));
          onItemDeleted();
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

  const [showDeleteConfirmation, hideDeleteConfirmation] = useModal(() => (
    <Confirm
      visible={true}
      title={t('Labels:DeleteItem')}
      onOk={() => handleDelete(contentItem, hideDeleteConfirmation)}
      onCancel={hideDeleteConfirmation}
    >
      {t('Messages:DeleteConfirm', { name: '$t(Labels:TheItem)' })}
    </Confirm>
  ), [contentItem]);

  return (
    <div className="card mb-3">
      <div className="card-body">
        <Formik 
          initialValues={contentItem} 
          onSubmit={onSubmit}
        > 
          {({ handleSubmit, values }) => {
            const localizableFields = collection.contentType.contentFields.filter(cf => cf.isLocalizable);
            const nonLocalizableFields = collection.contentType.contentFields.filter(cf => !cf.isLocalizable);
            const metaFields = collection.contentType.metaFields;
            return (        
              <form onSubmit={handleSubmit}>
                <div className="row">
                  <div className="col-3">
                    <Field name="contentKey" label={t('Labels:ContentKey')} component={TextInput} />
                    {nonLocalizableFields.map(field => {
                      const Editor = editorMap[field.fieldType];
                      return <Field key={field.name} name={`content.${field.name}`} label={field.description} component={Editor} />
                    })}
                    {metaFields.map(field => {
                      const Editor = editorMap[field.fieldType];
                      return <Field key={field.name} name={`meta.${field.name}`} label={field.description} component={Editor} />
                    })}
                  </div>
                  {activeLanguages.map(lang => 
                    <div className="col" key={lang}>
                      {localizableFields.map(field => {
                        const Editor = editorMap[field.fieldType];
                        return <Field key={field.name} name={`content.${field.name}.${lang}`} label={field.description} component={Editor} />
                      })}                
                    </div>
                  )}
                  <div className="col-2 d-flex flex-column">
                    <button type="submit" className="btn btn-primary btn-block"><FaSave className="mr-1" />{t('Labels:SaveButton')}</button>
                    <button type="button" className="btn btn-secondary btn-block" onClick={onClose}><FaTimes className="mr-1" />{t('Labels:CancelButton')}</button>
                    { ! isNew &&
                      <button type="button" className="btn btn-danger btn-block" onClick={showDeleteConfirmation}><FaTrash className="mr-1" />{t('Labels:DeleteButton')}</button>                  
                    }
                  </div>
                </div>
              </form>
            )
          }}
        </Formik>
      </div>
    </div>
  );
};

export default EditableListItem;
