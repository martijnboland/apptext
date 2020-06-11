import React, { useState } from 'react';
import { ContentType } from './models';
import { Link } from 'react-router-dom';
import { Formik, Field, FormikHelpers } from 'formik';
import { FaSave, FaTrash } from 'react-icons/fa';

import { TextInput } from '../common/components/form';
import { IApiResult } from '../common/api';
import Fields from './Fields';
import { useTranslation } from 'react-i18next';

interface EditFormProps {
  contentType?: ContentType,
  onSave: (contentType: ContentType) => Promise<IApiResult>,
  onDelete?: () => void
}

const EditForm: React.FunctionComponent<EditFormProps> = ({ contentType, onSave, onDelete }) => {
  const { t } = useTranslation('Labels');
  const onSubmit = (values: any, actions: FormikHelpers<any>) => {
    onSave(values)
      .then(res => {
        if (! res.ok) {
          actions.setErrors(res.errors);
        }
      });
  };

  const onDeleteContentType = () => {
    onDelete();
  }
  
  return (
    <Formik 
      initialValues={contentType} 
      onSubmit={onSubmit}
    > 
      {({ handleSubmit, values }) => (
        <form onSubmit={handleSubmit}>
          <Field name="name" label={t('Labels:Name')} component={TextInput} />
          <Field name="description" label={t('Labels:Description')} component={TextInput} />
          <Fields name="contentFields" label={t('Labels:ContentFields')} fields={values.contentFields} />
          <Fields name="metaFields" label={t('Labels:MetaFields')} fields={values.metaFields} />
          <div className="d-flex">
            <button type="submit" className="btn btn-primary mr-2"><FaSave className="mr-1" />{t('Labels:SaveButton')}</button>
            <Link to={{ pathname: '/contenttypes' }} className="btn btn-link">{t('Labels:CancelButton')}</Link>
            {contentType && contentType.id &&
              <button type="button" className="btn btn-danger ml-auto" onClick={onDeleteContentType}><FaTrash className="mr-1" />{t('Labels:DeleteButton')}</button>          
            }
          </div>
        </form>
      )}
    </Formik>
  );
};

export default EditForm;