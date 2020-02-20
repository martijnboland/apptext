import React, { useContext } from 'react';
import { Collection } from '../collections/models';
import { ContentItem, ContentItemCommand } from './models';
import { Formik, Field, FormikHelpers } from 'formik';
import { TextInput } from '../common/components/form';
import AppContext from '../apps/AppContext';
import { appConfig } from '../config/AppConfig';
import { useApi } from '../common/api';
import { toast } from 'react-toastify';

interface IEditableListItemProps {
  isNew: boolean,
  collection: Collection,
  contentItem?: ContentItem,
  activeLanguages: string[],
  onClose: () => void,
  onItemSaved: () => void
}

const EditableListItem: React.FunctionComponent<IEditableListItemProps> = ({ isNew, collection, contentItem, activeLanguages, onClose, onItemSaved }) => {

  const { currentApp } = useContext(AppContext);
  const url = `${appConfig.apiBaseUrl}/${currentApp.id}/content`;
  const contentItemApiAction = isNew 
    ? useApi<ContentItemCommand>(url, 'POST')
    : useApi<ContentItemCommand>(url + `/${contentItem.id}`, 'PUT');

  const onSubmit = (contentItem: ContentItem, actions: FormikHelpers<ContentItem>): Promise<any> => {
    const contentItemCommand: ContentItemCommand = { ...contentItem, languagesToValidate: activeLanguages };
    return contentItemApiAction.callApi(contentItemCommand)
      .then(res => {
        if (res.ok) {
          if (isNew) {
            toast.success(`Content item ${contentItem.contentKey} created`);
          } else {
            toast.success(`Content item ${contentItem.contentKey} updated`);
          }
          onItemSaved();
        } else {
          actions.setErrors(res.errors);
        }
        return res;
      });
  };

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
                    <Field name="contentKey" label="Key" component={TextInput} />
                    {nonLocalizableFields.map(field =>
                      <Field key={field.name} name={`content.${field.name}`} label={field.description} component={TextInput} />
                    )}
                    {metaFields.map(field =>
                      <Field key={field.name} name={`meta.${field.name}`} label={field.description} component={TextInput} />
                    )}
                  </div>
                  {activeLanguages.map(lang => 
                    <div className="col" key={lang}>
                      {localizableFields.map(field =>
                        <Field key={field.name} name={`content.${field.name}.${lang}`} label={field.description} component={TextInput} />
                      )}                
                    </div>
                  )}
                  <div className="col-2">
                    <button type="submit" className="btn btn-primary btn-block">Save</button>
                    <button type="button" className="btn btn-secondary btn-block" onClick={onClose}>Cancel</button>
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
