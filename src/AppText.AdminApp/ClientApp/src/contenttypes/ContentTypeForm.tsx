import React, { useState } from 'react';
import { ContentType } from './models';
import { Link } from 'react-router-dom';
import { Formik, Field, FieldArray } from "formik";
import { FaSave } from 'react-icons/fa';

import { TextInput } from '../common/components/form';
import { IApiResult } from '../common/api';
import Fields from './Fields';

interface EditFormProps {
  contentType?: ContentType,
  errors?: any,
  onSave: (contentType: ContentType) => Promise<IApiResult>
}

const EditForm: React.FunctionComponent<EditFormProps> = ({ contentType, onSave, errors }) => {
  
  const onSubmit = (values, actions) => {
    onSave(values)
      .then(res => {
        if (! res.ok) {
          actions.setErrors(res.errors);
        }
      });
  };
  
  return (
    <Formik initialValues={contentType}
      onSubmit={onSubmit}
      render={({ handleSubmit, values }) => (
        <form onSubmit={handleSubmit}>
          <Field name="name" label="Name" component={TextInput} />
          <Field name="description" label="Description" component={TextInput} />
          <Fields name="contentFields" label="Content fields" fields={values.contentFields} />
          <Fields name="metaFields" label="Meta fields" fields={values.metaFields} />
          <button type="submit" className="btn btn-primary mr-2"><FaSave className="mr-1" />Save</button>
          <Link to={{ pathname: '/contenttypes' }} className="btn btn-link">Cancel</Link>
        </form>
      )}>
    </Formik>
  );
};

export default EditForm;