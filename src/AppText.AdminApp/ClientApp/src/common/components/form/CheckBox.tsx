import React from 'react';
import { FieldProps } from 'formik';
import { ICustomFieldProps } from './ICustomFieldProps';

export const CheckBox: React.FunctionComponent<FieldProps & ICustomFieldProps> = ({ label, className, field, }) => {
  const cssClass = className || 'form-check';

  return (
    <div className={cssClass}>
      <label className="form-check-label">
        <input {...field} type="checkbox" checked={field.value} className="form-check-input" />
        {label}
      </label>
    </div>
  );
};