import React from 'react';
import classNames from 'classnames';

import { FieldProps, getIn } from 'formik';
import { ICustomFieldProps } from './ICustomFieldProps';

interface TextAreaProps extends ICustomFieldProps {
}

export const TextArea: React.FC<TextAreaProps & FieldProps> = ({ 
  label,
  className,
  field, // { name, value, onChange, onBlur }
  form: { touched, errors }, // also values, setXXXX, handleXXXX, dirty, isValid, status, etc.
  ...rest
}) => {

  const cssClass = className || 'form-group';
  const error = getIn(errors, field.name);
  const touch = getIn(touched, field.name);
  const { value, ...restField } = field;

  return (
    <div className={cssClass}>
      <label htmlFor={field.name}>{label}</label>
      <textarea {...restField} value={value||''} {...rest} className={classNames('form-control', { 'is-invalid': error })}>{value}</textarea>
      {error && <div className="invalid-feedback">{error}</div>}
    </div>
  );

}

export const parseStringArrayFromTextArea = (value: string): string[] => {
  if (! value) {
    return [];
  }
  // split on newlines
  return value.split(/[\r\n]+/);
}

export const formatStringArrayToTextArea = (values: string[]): string => {
  if (! values) {
    return '';
  }
  return values.join("\n");
}