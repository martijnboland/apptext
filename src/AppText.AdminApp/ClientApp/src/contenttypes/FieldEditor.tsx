import React from 'react';
import { FieldProps, Field } from 'formik';
import { TextInput, Select, CheckBox, SelectOption } from '../common/components/form';
import { useTranslation } from 'react-i18next';

interface FieldEditorProps {
  isNew: boolean,
  onClose: () => void,
  onRemove: () => void
}

const FieldEditor: React.FunctionComponent<FieldProps & FieldEditorProps> = ({ field, form, isNew, onClose, onRemove, ...rest }) => {
  const { t } = useTranslation('Labels'); 
  const initialValue = field.value || {};
  const options: SelectOption[] = [
    { value: 'ShortText', label: t('Labels:ShortText') },
    { value: 'LongText', label: t('Labels:LongText') },
    { value: 'DateTime', label: t('Labels:DateTime') },
    { value: 'Number', label: t('Labels:Number') }
  ]
  return (
    <div className="card">
      <div className="card-body">
        <div className="form-row">
          <Field name={field.name + '.name'} label={t('Labels:Name')} className="form-group col-md-4" component={TextInput} />
          <Field name={field.name + '.fieldType'} label={t('Labels:FieldType')} className="form-group col-md-4" component={Select} options={options} />
          <div className="form-group col-md-4">
            <label>&nbsp;</label>
            <Field name={field.name + '.isRequired'} label={t('Labels:Required')} component={CheckBox} />
          </div>
        </div>
        <Field name={field.name + '.description'} label={t('Labels:Description')} component={TextInput} />
        <button className="btn btn-primary mr-2" type="button" onClick={onClose}>{t('Labels:OkButton')}</button>
        <button className="btn btn-danger" type="button" onClick={onRemove}>{t('Labels:RemoveButton')}</button>
      </div>
    </div>
  );
};

export default FieldEditor;