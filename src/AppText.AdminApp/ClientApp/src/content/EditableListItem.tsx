import React from 'react';
import { Collection } from '../collections/models';
import { ContentItem } from './models';
import { Formik, Field } from 'formik';
import { TextInput } from '../common/components/form';

interface IEditableListItemProps {
  isNew: boolean,
  collection: Collection,
  contentItem?: ContentItem,
  activeLanguages: string[],
  hasMoreLanguages: boolean,
  onClose: () => void
}

const EditableListItem: React.FunctionComponent<IEditableListItemProps> = ({ collection, contentItem, activeLanguages, hasMoreLanguages, onClose }) => {

  const onSubmit = (values: ContentItem) => {
    console.log('ContentItem => ', values);
  }

  return (
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
            <div className="row border-bottom">
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
              {hasMoreLanguages && 
                <div className="col"></div>
              }
              <div className="col-1">
                <button type="submit" className="btn btn-primary mr-1">Save</button>
                <button type="button" className="btn btn-secondary" onClick={onClose}>Cancel</button>
              </div>
            </div>
          </form>
        )
      }}
    </Formik>
  );
};

export default EditableListItem;
