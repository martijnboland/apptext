import React from 'react';
import classNames from 'classnames';
import { FieldProps } from 'formik';
import { ICustomFieldProps } from './ICustomFieldProps';

interface TextInputProps extends ICustomFieldProps {
  type?: string,
}

export const TextInput: React.FunctionComponent<FieldProps & TextInputProps> = ({
  type,
  label,
  className,
  field, // { name, value, onChange, onBlur }
  form: { touched, errors }, // also values, setXXXX, handleXXXX, dirty, isValid, status, etc.
  ...rest
}) => {
  const inputType = type || 'text';
  const cssClass = className || 'form-group';
  return (
    <div className={cssClass}>
      <label>{label}</label>
      <input type={inputType} {...field} {...rest} className={classNames('form-control', { 'is-invalid': errors[field.name] })} />
      {errors[field.name] && touched[field.name] && <div className="invalid-feedback">{errors[field.name]}</div>}
    </div>
  );
};