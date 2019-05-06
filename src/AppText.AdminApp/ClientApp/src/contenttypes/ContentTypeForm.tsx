import React from 'react';
import { ContentType, Field as ContentTypeField } from './models';
import { Link } from 'react-router-dom';
import FieldEditor from './FieldEditor';
import { Formik, Field, FieldArray } from "formik";
import { TextInput } from '../common/components/form';
import { IApiResult } from '../common/api';

interface FieldsProps {
  name: string,
  label: string,
  fields: ContentTypeField[]
}

const Fields: React.FunctionComponent<FieldsProps> = ({ name, label, fields }) => {
  const newField: ContentTypeField = {
    name: '',
    fieldType: 'ShortText',
    isRequired: false,
    description: ''
  };

  return (
    <FieldArray name={name} render={arrayHelpers => (
      <div className="form-group">
        <label>{label}</label>
        <ul>
          {fields.map((field, idx) => {
            const key = `name_${idx}`;
            return (
              <li key={key}>
                <Field name={`${name}[${idx}]`} component={FieldEditor} />
                <button className="btn btn-primary" type="button" onClick={() => arrayHelpers.remove(idx)}>-</button>
              </li>
            );
          })}
          <li>
            <button className="btn btn-primary" type="button" onClick={() => arrayHelpers.push({...newField})}>+</button>
          </li>
        </ul>
      </div>
    )} />
  );
}

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
          <button type="submit" className="btn btn-primary mr-2">Save</button>
          <Link to={{ pathname: '/contenttypes' }}>Cancel</Link>
        </form>
      )}>
    </Formik>
  );
};

export default EditForm;