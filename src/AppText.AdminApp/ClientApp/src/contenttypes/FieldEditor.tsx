import React from 'react';
import { FieldProps, Field } from 'formik';
import { TextInput, Select, CheckBox, SelectOption } from '../common/components/form';

interface FieldEditorProps {
  isNew: boolean,
  onClose: () => void,
  onRemove: () => void
}

const FieldEditor: React.FunctionComponent<FieldProps & FieldEditorProps> = ({ field, form, isNew, onClose, onRemove, ...rest }) => {
  const initialValue = field.value || {};
  const options: SelectOption[] = [
    { value: 'ShortText', label: 'Short text' },
    { value: 'LongText', label: 'Long text' },
    { value: 'DateTime', label: 'Date and time' },
    { value: 'Number', label: 'Number' }
  ]
  return (
    <div className="card">
      <div className="card-body">
        <div className="form-row">
          <Field name={field.name + '.name'} label="Name" className="form-group col-md-4" component={TextInput} />
          <Field name={field.name + '.fieldType'} label="Field type" className="form-group col-md-4" component={Select} options={options} />
          <div className="form-group col-md-4">
            <label>&nbsp;</label>
            <Field name={field.name + '.isRequired'} label="Required" component={CheckBox} />
          </div>
        </div>
        <Field name={field.name + '.description'} label="Description" component={TextInput} />
        <button className="btn btn-primary mr-2" type="button" onClick={onClose}>Ok</button>
        <button className="btn btn-danger" type="button" onClick={onRemove}>Remove</button>
      </div>
    </div>
  );
};

export default FieldEditor;