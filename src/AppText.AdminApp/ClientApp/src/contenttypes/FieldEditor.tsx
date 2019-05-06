import React from 'react';
import { FieldProps, Field } from 'formik';
import { TextInput, Select, CheckBox, SelectOption } from '../common/components/form';

const FieldEditor: React.FunctionComponent<FieldProps> = ({ field, form, ...rest }) => {
  const initialValue = field.value || {};
  const options: SelectOption[] = [
    { value: 'ShortText', description: 'Short text' },
    { value: 'LongText', description: 'Long text' },
    { value: 'DateTime', description: 'Date and time' },
    { value: 'Number', description: 'Number' }
  ]
  return (
    <>
      <div className="form-row">
        <Field name={field.name + '.name'} label="Name" className="form-group col-md-4" component={TextInput} />
        <Field name={field.name + '.fieldType'} label="Field type" className="form-group col-md-4" component={Select} options={options} />
        <div className="form-group col-md-4">
          <label>&nbsp;</label>
          <Field name={field.name + '.isRequired'} label="Required" component={CheckBox} />
        </div>
      </div>
      <Field name={field.name + '.description'} label="Description" component={TextInput} />
    </>
  );
};

export default FieldEditor;