import React, { useState } from 'react';
import { FieldArray, Field, getIn } from 'formik';
import { FaPlus } from 'react-icons/fa';

import { Field as ContentTypeField } from './models';
import FieldEditor from './FieldEditor';
import DisplayField from './DisplayField';

interface FieldsProps {
  name: string,
  label: string,
  fields: ContentTypeField[]
}

const Fields: React.FunctionComponent<FieldsProps> = ({ name, label, fields }) => {

  const initialDisplayState = fields.map(() => { return { isNew: false, isEdit: false } } )

  const [displayState, setDisplayState] = useState(initialDisplayState);

  const newField: ContentTypeField = {
    name: '',
    fieldType: 'ShortText',
    isRequired: false,
    description: ''
  };

  return (
    <FieldArray name={name} render={arrayHelpers => {

      const addItem = () => {
        arrayHelpers.push({...newField});
        setDisplayState([...displayState, { isNew: true, isEdit: true}]);
      }

      const removeItem = (idx: number) => {
        arrayHelpers.remove(idx);
        setDisplayState([...displayState.slice(0, idx), ...displayState.slice(idx + 1, displayState.length)]);
      }

      const selectItem = (idx: number) => {
        displayState[idx].isEdit = true;
        setDisplayState([...displayState]);
      }

      const closeItem = (idx: number) => {
        displayState[idx].isEdit = false;
        setDisplayState([...displayState]);
      }

      return (
        <div className="form-group">
          <label>{label}</label>
          <ul className="list-unstyled">
            {fields.map((field, idx) => {
              const key = `name_${idx}`;
              const fieldName = `${name}[${idx}]`;
              const error = getIn(arrayHelpers.form.errors, fieldName);
              return (
                <li key={key} className="mb-2">
                  {displayState[idx].isEdit || error
                    ?
                      <Field name={fieldName} component={FieldEditor} onClose={() => closeItem(idx)} onRemove={() => removeItem(idx)} isNew={displayState[idx].isNew} />
                    :
                      <DisplayField field={field} onSelect={() => selectItem(idx)} />
                  }
                </li>
              );
            })}
            <li>
              <button className="btn btn-secondary" type="button" onClick={addItem}>
                <FaPlus className="mr-1" />Add field
              </button>
            </li>
          </ul>
        </div>
      )
    }} />
  );
}

export default Fields;