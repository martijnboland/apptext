import React, { useState } from 'react';
import { ContentType } from './models';
import { Link } from 'react-router-dom';
import { Formik, Field, FieldArray, FormikActions } from "formik";
import { FaSave, FaTrash } from 'react-icons/fa';

import { TextInput } from '../common/components/form';
import { IApiResult } from '../common/api';
import Fields from './Fields';

interface EditFormProps {
  contentType?: ContentType,
  onSave: (contentType: ContentType) => Promise<IApiResult>,
  onDelete?: () => void
}

const EditForm: React.FunctionComponent<EditFormProps> = ({ contentType, onSave, onDelete }) => {
  
  const onSubmit = (values: any, actions: FormikActions<any>) => {
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
    <Formik initialValues={contentType}
      onSubmit={onSubmit}
      render={({ handleSubmit, values }) => (
        <form onSubmit={handleSubmit}>
          <Field name="name" label="Name" component={TextInput} />
          <Field name="description" label="Description" component={TextInput} />
          <Fields name="contentFields" label="Content fields" fields={values.contentFields} />
          <Fields name="metaFields" label="Meta fields" fields={values.metaFields} />
          <div className="d-flex">
            <button type="submit" className="btn btn-primary mr-2"><FaSave className="mr-1" />Save</button>
            <Link to={{ pathname: '/contenttypes' }} className="btn btn-link">Cancel</Link>
            {contentType && contentType.id &&
              <button type="button" className="btn btn-danger ml-auto" onClick={onDeleteContentType}><FaTrash className="mr-1" />Delete</button>          
            }
          </div>
        </form>
      )}>
    </Formik>
  );
};

export default EditForm;