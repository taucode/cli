(executor
    (repeatable
        (alternatives

            ;===== Host =====
            (key-value
                :keys ("-h" "--host")
                :alias "host"
                :token-types ("host-name")
            )

            ;===== Port =====
            (key-value
                :keys ("--port")
                :alias "port"
                :token-types ("integer")
            )

            ;===== Username =====
            (key-value
                :keys ("-u" "--username")
                :alias "username"
                :token-types ("string" "email-address")
            )

            ;===== Password =====
            (key-value
                :keys ("-p" "--password")
                :alias "password"
                :token-types ("string")
            )

            ;===== To =====
            (key-value
                :keys ("-t" "--to")
                :alias "to"
                :token-types ("email-address" "string")
            )

            ;===== Cc =====
            (key-value
                :keys ("--cc")
                :alias "cc"
                :token-types ("email-address" "string")
            )

            ;===== Bcc =====
            (key-value
                :keys ("--bcc")
                :alias "bcc"
                :token-types ("email-address" "string")
            )

            ;===== Subject =====
            (key-value
                :keys ("-s" "--subject")
                :alias "subject"
                :token-types ("string")
            )

            ;===== Subject =====
            (key-value
                :keys ("-m" "--message")
                :alias "message"
                :token-types ("string")
            )

            ;===== Attachment =====
            (sequence
                (custom-key
                    :is-entrance t
                    :keys ("-a" "--attachment")
                    :name "attachment-key"
                )

                (some-text
                    :token-types ("file-path" "string")
                    :name "attachment-file-path"
                )

                (optional
                    (some-text
                        :token-types ("file-path" "string")
                        :name "attachment-file-local-name"
                    )
                )

                (optional
                    :is-exit t

                    (boolean
                        :name "attachment-file-is-inline"
                    )
                )
            )

            ;===== Fallback =====
            (fallback)
        )
    )
    (end)
)
