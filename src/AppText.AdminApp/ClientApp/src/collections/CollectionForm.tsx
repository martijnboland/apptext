import React, { useState } from 'react';
import { Collection } from './models';
import { Link } from 'react-router-dom';
import { Formik, Field, FormikHelpers } from 'formik';
import { FaSave, FaTrash } from 'react-icons/fa';

import { TextInput, Select, SelectOption, TextArea } from '../common/components/form';
import { IApiResult } from '../common/api';
import { ContentType } from '../contenttypes/models';
import { useTranslation } from 'react-i18next';

interface CollectionFormProps {
  collection?: Collection,
  contentTypes?: ContentType[]
  onSave: (collection: Collection) => Promise<IApiResult>,
  onDelete?: () => void
}

const CollectionForm: React.FC<CollectionFormProps> = ({ collection, contentTypes, onSave, onDelete }) => {
  const { t } = useTranslation('Labels');
  const initialContentType = collection.contentType || (contentTypes.length > 0 ? contentTypes[0] : undefined);
  const [ currentContentType, setCurrentContentType ] = useState(initialContentType);
  if (currentContentType === undefined && initialContentType !== undefined) {
    setCurrentContentType(initialContentType);
  }

  const contentTypeOptions = contentTypes.map(ct => { return { value: ct.id, label: ct.name } });
  const contentFieldOptions = currentContentType ? currentContentType.contentFields.map(cf => { return { value: cf.name, label: cf.name } }) : [];
  const initialValues = { 
    ...collection, 
    contentType: collection.contentType ? collection.contentType.id : undefined, 
    listDisplayField: collection.listDisplayField || undefined
  };

  const onSubmit = (values: any, actions: FormikHelpers<any>) => {
    // Set the full content type before storing
    const collection = { ...values };
    collection.contentType = contentTypes.find(ct => ct.id === values.contentType );
    onSave(collection)
      .then(res => {
        if (! res.ok) {
          actions.setErrors(res.errors);
        }
      });
  };

  const onDeleteCollection = () => {
    onDelete();
  }

  const onContentTypeChanged = (contentTypeId) => {
    setCurrentContentType(contentTypes.find(ct => ct.id === contentTypeId));
  }
  
  return (
    <Formik 
      enableReinitialize 
      initialValues={initialValues}
      onSubmit={onSubmit}
    >
      {({ handleSubmit, values }) => (
        <form onSubmit={handleSubmit}>
          <Field name="name" label={t('Labels:Name')} component={TextInput} />
          <Field name="description" label={t('Labels:Description')} component={TextArea} />
          <Field name="contentType" label={t('Labels:ContentType')} component={Select} options={contentTypeOptions} onChange={onContentTypeChanged} />
          <Field name="listDisplayField" label={t('Labels:ListDisplayField')} component={Select} options={contentFieldOptions} insertEmpty />
          <div className="d-flex">
            <button type="submit" className="btn btn-primary mr-2"><FaSave className="mr-1" />{t('Labels:SaveButton')}</button>
            <Link to={{ pathname: '/' }} className="btn btn-link">{t('Labels:CancelButton')}</Link>
            {collection && collection.id &&
              <button type="button" className="btn btn-danger ml-auto" onClick={onDeleteCollection}><FaTrash className="mr-1" />{t('Labels:DeleteButton')}</button>          
            }
          </div>
        </form>
      )}
    </Formik>
  );
};

export default CollectionForm;